using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEnvironment : GDictionary
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    public MechBag MechBag { get; private set; }
    public SkillInventory SkillInventory { get; private set; }

    public RunEntity Hero { get; private set; }
    private RunEntity _enemy;
    public RunEntity Enemy
    {
        get => _enemy;
        set
        {
            if (_enemy != null) _enemy.EnvironmentChangedEvent -= EnvironmentChanged;
            _enemy = value;
            if (_enemy != null) _enemy.EnvironmentChangedEvent += EnvironmentChanged;
            EnvironmentChanged();
        }
    }

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public RunEnvironment()
    {
        _accessors = new()
        {
            { "MechBag",               () => MechBag },
            { "SkillInventory",        () => SkillInventory },
            { "Hero",                  () => Hero },
            { "Enemy",                 () => Enemy },
        };

        MechBag = new();
        SkillInventory = new();

        Hero = new();
        Hero.EnvironmentChangedEvent += EnvironmentChanged;

        EnvironmentChangedEvent += CalcReport;
        EnvironmentChangedEvent += CalcManaShortageBrief;
    }

    public void Enter()
    {
        CreateEntityDetails d = new CreateEntityDetails(RunManager.Instance.Map.JingJie);
        RunManager.Instance.EntityPool.ForceDrawEntityEntry(out EntityEntry entry, d);
        Enemy = new RunEntity(entry, d);
    }

    public bool TryMerge(RunSkill lhs, RunSkill rhs)
    {
        // if (lhs.GetJingJie() >= Hero.GetJingJie() && lhs.Entry != rhs.Entry)
        //     return false;

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
            EnvironmentChanged();
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
        EnvironmentChanged();
        return true;
    }

    public bool TryEquip(RunSkill toEquip, SkillSlot slot)
    {
        RunSkill toUnequip = slot.Skill;

        if (toUnequip != null)
            SkillInventory.ReplaceSkill(toEquip, toUnequip);
        else
            SkillInventory.RemoveSkill(toEquip);

        slot.Skill = toEquip;
        EnvironmentChanged();
        return true;
    }

    public bool TryUnequip(SkillSlot slot, object _)
    {
        RunSkill toUnequip = slot.Skill;
        if (toUnequip == null)
            return false;

        slot.Skill = null;
        SkillInventory.AddSkill(toUnequip);
        EnvironmentChanged();
        return true;
    }

    public bool TrySwap(SkillSlot fromSlot, SkillSlot toSlot)
    {
        RunSkill temp = fromSlot.Skill;
        fromSlot.Skill = toSlot.Skill;
        toSlot.Skill = temp;
        return true;
    }

    public bool TryWrite(RunSkill fromSkill, SkillSlot toSlot)
    {
        toSlot.Skill = fromSkill;
        return true;
    }

    public bool TryWrite(SkillSlot fromSlot, SkillSlot toSlot)
    {
        toSlot.Skill = fromSlot.Skill;
        return true;
    }

    public bool TryIncreaseJingJie(RunSkill skill)
    {
        bool success = skill.TryIncreaseJingJie();
        if (!success)
            return false;
        EnvironmentChanged();
        return false;
    }

    public bool TryIncreaseJingJie(SkillSlot slot)
    {
        bool success = slot.TryIncreaseJingJie();
        if (!success)
            return false;
        EnvironmentChanged();
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
