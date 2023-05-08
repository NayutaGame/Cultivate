using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnvironment : GDictionary
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    public SkillInventory SkillInventory { get; private set; }

    public RunEntity Hero { get; private set; }
    private RunEntity _enemy;
    public RunEntity Enemy
    {
        get => _enemy;
        set // Entity 的 SetEntry 的具体实现
        {
            if (_enemy != null)
            {
                _enemy.SetDragDropDelegate(null);
                _enemy.EnvironmentChangedEvent -= EnvironmentChanged;
            }
            _enemy = value;
            _enemy.SetDragDropDelegate(_dragDropDelegate);
            _enemy.EnvironmentChangedEvent += EnvironmentChanged;
            EnvironmentChanged();
        }
    }

    private DragDropDelegate _dragDropDelegate;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public RunEnvironment()
    {
        _accessors = new()
        {
            { "SkillInventory",        () => SkillInventory },
            { "Hero",                  () => Hero },
            { "Enemy",                 () => Enemy },
        };

        _dragDropDelegate = new(4, new Func<IDragDrop, IDragDrop, bool>[]
            {
                /*                     RunSkill,   SkillInventory, SkillSlot(Hero), SkillSlot(Enemy) */
                /* RunSkill         */ TryMerge,   null,           TryEquip,        null,
                /* SkillInventory   */ null,       null,           null,            null,
                /* SkillSlot(Hero)  */ TryUnequip, TryUnequip,     TrySwap,         null,
                /* SkillSlot(Enemy) */ null,       null,           null,            null,
            },
            item =>
            {
                if (item is RunSkill)
                    return 0;
                if (item is SkillInventory)
                    return 1;
                if (item is SkillSlot skillSlot)
                {
                    if (skillSlot.Owner == Hero)
                        return 2;
                    if (skillSlot.Owner == Enemy)
                        return 3;
                }

                throw new Exception();
            });

        SkillInventory = new();
        SkillInventory.SetDragDropDelegate(_dragDropDelegate);

        Hero = new();
        Hero.SetDragDropDelegate(_dragDropDelegate);

        EnvironmentChangedEvent += CalcReport;
        EnvironmentChangedEvent += CalcManaShortageBrief;
    }

    public void Enter()
    {
        CreateEntityDetails d = new CreateEntityDetails(RunManager.Instance.Map.JingJie);
        RunManager.Instance.EntityPool.TryDrawEntityEntry(out EntityEntry entry, d);
        Enemy = new RunEntity(entry, d);
    }

    private bool TryMerge(IDragDrop from, IDragDrop to)
    {
        RunSkill lhs = from as RunSkill;
        RunSkill rhs = to as RunSkill;

        if (lhs.GetJingJie() != rhs.GetJingJie())
            return false;

        JingJie jingJie = lhs.GetJingJie();
        WuXing? lWuXing = lhs._entry.WuXing;
        WuXing? rWuXing = rhs._entry.WuXing;

        bool upgrade;

        if (jingJie == JingJie.FanXu) {
            upgrade = false;
        } else if (jingJie == JingJie.HuaShen) {
            // upgrade = RandomManager.value < 0.05;
            upgrade = false;
        } else {
            upgrade = true;
        }

        JingJie newJingJie = jingJie + (upgrade ? 1 : 0);

        if (lhs._entry == rhs._entry && upgrade && lhs._entry.JingJieRange.Contains(lhs.JingJie + 1))
        {
            rhs.JingJie = newJingJie;
            SkillInventory.Remove(lhs);
            return true;
        }
        else if (!lWuXing.HasValue || !rWuXing.HasValue)
        {
            return false;
        }

        Predicate<SkillEntry> pred = null;
        WuXing? wuXing = null;

        if (WuXing.SameWuXing(lWuXing, rWuXing))
        {
            pred = s => s != lhs._entry && s != rhs._entry;
            wuXing = lWuXing;
        }
        else if (WuXing.XiangSheng(lWuXing, rWuXing))
        {
            wuXing = WuXing.XiangShengNext(lWuXing, rWuXing).Value;
        }
        else
        {
            pred = s => s.WuXing.HasValue && s.WuXing != lWuXing && s.WuXing != rWuXing;
        }

        bool success = RunManager.Instance.SkillPool.TryDrawSkill(out RunSkill newSkill, pred, wuXing, newJingJie);
        if (!success)
            return false;

        SkillInventory.Replace(rhs, newSkill);
        SkillInventory.Remove(lhs);
        return true;
    }

    private bool TryEquip(IDragDrop from, IDragDrop to)
    {
        RunSkill toEquip = from as RunSkill;
        SkillSlot slot = to as SkillSlot;

        RunSkill toUnequip = slot.Skill;

        if (toUnequip != null)
            SkillInventory.Replace(toEquip, toUnequip);
        else
            SkillInventory.Remove(toEquip);

        slot.Skill = toEquip;
        EnvironmentChanged();
        return true;
    }

    public bool TryUnequip(IDragDrop from, IDragDrop to)
    {
        SkillSlot slot = from as SkillSlot;
        RunSkill toUnequip = slot.Skill;
        if (toUnequip == null)
            return false;

        slot.Skill = null;
        SkillInventory.Add(toUnequip);
        EnvironmentChanged();
        return true;
    }

    public bool TrySwap(IDragDrop from, IDragDrop to)
    {
        SkillSlot fromSlot = from as SkillSlot;
        SkillSlot toSlot = to as SkillSlot;

        RunSkill temp = fromSlot.Skill;
        fromSlot.Skill = toSlot.Skill;
        toSlot.Skill = temp;
        return true;
    }

    public StageReport Report;
    private void CalcReport()
    {
        Report = StageManager.SimulateBrief(Hero, Enemy);
    }

    [NonSerialized] public bool[] ManaShortageBrief;
    private void CalcManaShortageBrief()
        => ManaShortageBrief = StageManager.ManaSimulate(Hero, Enemy);
}
