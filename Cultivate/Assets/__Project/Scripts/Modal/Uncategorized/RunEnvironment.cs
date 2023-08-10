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
        WuXing? lWuXing = lhs.GetEntry().WuXing;
        WuXing? rWuXing = rhs.GetEntry().WuXing;

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

        if (lhs.GetEntry() == rhs.GetEntry() && upgrade && lhs.GetEntry().JingJieRange.Contains(lhs.JingJie + 1))
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
            pred = s => s != lhs.GetEntry() && s != rhs.GetEntry();
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

    public bool TryEquipSkill(RunSkill toEquip, SkillSlot slot)
    {
        EmulatedSkill toUnequip = slot.Skill;

        if (toUnequip == null)
            SkillInventory.RemoveSkill(toEquip);
        else if (toUnequip is RunSkill runSkill)
            SkillInventory.ReplaceSkill(toEquip, runSkill);
        else if (toUnequip is MechComposite mechComposite)
        {
            SkillInventory.RemoveSkill(toEquip);
            foreach(MechType m in mechComposite.MechTypes)
                MechBag.AddMech(m);
        };

        slot.Skill = toEquip;
        EnvironmentChanged();
        return true;
    }

    public bool TryEquipMech(Mech toEquip, SkillSlot slot)
    {
        EmulatedSkill currentSkill = slot.Skill;

        if (currentSkill == null)
        {
            bool success = MechBag.TryConsumeMech(toEquip.GetMechType());
            if (!success)
                return false;
            slot.Skill = new MechComposite(toEquip.GetMechType());
        }
        else if (currentSkill is RunSkill runSkill)
        {
            bool success = MechBag.TryConsumeMech(toEquip.GetMechType());
            if (!success)
                return false;
            SkillInventory.AddSkill(runSkill);
            slot.Skill = new MechComposite(toEquip.GetMechType());
        }
        else if (currentSkill is MechComposite mechComposite)
        {
            bool success = MechBag.TryConsumeMech(toEquip.GetMechType());
            if (!success)
                return false;

            if (mechComposite.MechTypes.Count >= MechComposite.MAX_CHAIN)
                return false;

            mechComposite.MechTypes.Add(toEquip.GetMechType());
        }

        EnvironmentChanged();
        return true;
    }

    public bool TryUnequip(SkillSlot slot, object _)
    {
        EmulatedSkill toUnequip = slot.Skill;
        if (toUnequip == null)
            return false;

        if (toUnequip is RunSkill runSkill)
        {
            SkillInventory.AddSkill(runSkill);

            slot.Skill = null;
            EnvironmentChanged();
            return true;
        }
        else if (toUnequip is MechComposite mechComposite)
        {
            foreach (var m in mechComposite.MechTypes)
                MechBag.AddMech(m);

            slot.Skill = null;
            EnvironmentChanged();
            return true;
        }

        return false;
    }

    public bool TrySwap(SkillSlot fromSlot, SkillSlot toSlot)
    {
        EmulatedSkill temp = fromSlot.Skill;
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
