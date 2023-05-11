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
            _enemy.SetDragDropDelegate(_interactDelegate);
            _enemy.EnvironmentChangedEvent += EnvironmentChanged;
            EnvironmentChanged();
        }
    }

    protected InteractDelegate _interactDelegate;

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

        InitDragDropDelegate();

        SkillInventory = new();
        SkillInventory.SetInteractDelegate(_interactDelegate);

        Hero = new();
        Hero.SetDragDropDelegate(_interactDelegate);
        Hero.EnvironmentChangedEvent += EnvironmentChanged;

        EnvironmentChangedEvent += CalcReport;
        EnvironmentChangedEvent += CalcManaShortageBrief;
    }

    public virtual void InitDragDropDelegate() { }

    public void Enter()
    {
        CreateEntityDetails d = new CreateEntityDetails(RunManager.Instance.Map.JingJie);
        RunManager.Instance.EntityPool.TryDrawEntityEntry(out EntityEntry entry, d);
        Enemy = new RunEntity(entry, d);
    }

    protected bool TryMerge(IInteractable from, IInteractable to)
    {
        RunSkill lhs = from as RunSkill;
        RunSkill rhs = to as RunSkill;

        if (lhs.GetJingJie() != rhs.GetJingJie())
            return false;

        JingJie jingJie = lhs.GetJingJie();
        WuXing? lWuXing = lhs.Entry.WuXing;
        WuXing? rWuXing = rhs.Entry.WuXing;

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

        if (lhs.Entry == rhs.Entry && upgrade && lhs.Entry.JingJieRange.Contains(lhs.JingJie + 1))
        {
            rhs.JingJie = newJingJie;
            SkillInventory.RemoveSkill(lhs);
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
            pred = s => s != lhs.Entry && s != rhs.Entry;
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

        SkillInventory.ReplaceSkill(rhs, newSkill);
        SkillInventory.RemoveSkill(lhs);
        return true;
    }

    protected bool TryEquip(IInteractable from, IInteractable to)
    {
        RunSkill toEquip = from as RunSkill;
        SkillSlot slot = to as SkillSlot;

        RunSkill toUnequip = slot.Skill;

        if (toUnequip != null)
            SkillInventory.ReplaceSkill(toEquip, toUnequip);
        else
            SkillInventory.RemoveSkill(toEquip);

        slot.Skill = toEquip;
        EnvironmentChanged();
        return true;
    }

    protected bool TryUnequip(IInteractable from, IInteractable to)
    {
        SkillSlot slot = from as SkillSlot;
        RunSkill toUnequip = slot.Skill;
        if (toUnequip == null)
            return false;

        slot.Skill = null;
        SkillInventory.AddSkill(toUnequip);
        EnvironmentChanged();
        return true;
    }

    protected bool TrySwap(IInteractable from, IInteractable to)
    {
        SkillSlot fromSlot = from as SkillSlot;
        SkillSlot toSlot = to as SkillSlot;

        RunSkill temp = fromSlot.Skill;
        fromSlot.Skill = toSlot.Skill;
        toSlot.Skill = temp;
        return true;
    }

    protected bool TryWrite(IInteractable from, IInteractable to)
    {
        RunSkill skill = null;
        if (from is RunSkill fromSkill)
        {
            skill = fromSkill;
        }
        else if (from is SkillSlot skillSlot)
        {
            skill = skillSlot.Skill;
        }

        SkillSlot toSlot = to as SkillSlot;
        toSlot.Skill = skill;
        return true;
    }

    protected bool TryIncreaseJingJie(IInteractable item)
    {
        if (item is RunSkill fromSkill)
        {
            return fromSkill.TryIncreaseJingJie();
        }
        else if (item is SkillSlot skillSlot)
        {
            return skillSlot.TryIncreaseJingJie();
        }
        return false;
    }

    public StageReport Report;
    private void CalcReport()
    {
        Report = StageManager.SimulateBrief(Hero, Enemy);
    }

    private void CalcManaShortageBrief()
    {
        bool[] manaShortageBrief = StageManager.ManaSimulate(Hero, Enemy);
        for (int i = 0; i < manaShortageBrief.Length; i++)
            Hero.GetSlot(i).IsManaShortage = manaShortageBrief[i];
    }
}
