
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class RunEnvironment : Addressable
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    public RunEntity Home { get; private set; }
    public RunEntity Away { get; set; }

    public Map Map { get; private set; }

    public TechInventory TechInventory { get; private set; }

    public SkillPool SkillPool { get; private set; }
    public SkillInventory SkillInventory { get; private set; }
    public MechBag MechBag { get; private set; }

    public EntityPool EntityPool { get; private set; }

    public StageEnvironmentResult SimulateResult;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public RunEnvironment()
    {
        _accessors = new()
        {
            { "Hero",                  () => Home },
            { "Map",                   () => Map },
            { "TechInventory",         () => TechInventory },
            { "SkillInventory",        () => SkillInventory },
            { "MechBag",               () => MechBag },
        };

        Home = RunEntity.Default;
        Home.EnvironmentChangedEvent += EnvironmentChanged;

        Map = new();
        TechInventory = new();

        SkillPool = new();
        SkillInventory = new();
        MechBag = new();
        EntityPool = new();

        EnvironmentChangedEvent += Simulate;

        _turn = 1;
        _xiuWei = 0;
    }

    public void Combat()
    {
        StageEnvironmentDetails d = new StageEnvironmentDetails(true, true, false, false, RunManager.Instance.Battle.Home, Away);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
    }

    public void Simulate()
    {
        RunEntity away = Away ?? RunEntity.FromJingJieHealth(Home.GetJingJie(), 1000000);
        StageEnvironmentDetails d = new StageEnvironmentDetails(false, false, false, false, Home, away);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
        SimulateResult = environment.Result;
    }

    public void Enter()
    {
        // CreateEntityDetails d = new CreateEntityDetails(RunManager.Instance.Map.JingJie);
        // RunManager.Instance.EntityPool.ForceDrawEntityEntry(out EntityEntry entry, d);
        // Enemy = new RunEntity(entry, d);
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
            SkillInventory.Remove(lhs);
            SkillInventory.SetModified(rhs);
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

        bool success = SkillPool.TryDrawSkill(out RunSkill newSkill, pred, wuXing, newJingJie);
        if (!success)
            return false;

        SkillInventory.Replace(rhs, newSkill);
        SkillInventory.Remove(lhs);
        EnvironmentChanged();
        return true;
    }

    public bool TryEquipSkill(RunSkill toEquip, SkillSlot slot)
    {
        EmulatedSkill toUnequip = slot.Skill;

        if (toUnequip == null)
            SkillInventory.Remove(toEquip);
        else if (toUnequip is RunSkill runSkill)
            SkillInventory.Replace(toEquip, runSkill);
        else if (toUnequip is MechComposite mechComposite)
        {
            SkillInventory.Remove(toEquip);
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
            SkillInventory.Add(runSkill);
            slot.Skill = new MechComposite(toEquip.GetMechType());
        }
        else if (currentSkill is MechComposite mechComposite)
        {
            if (mechComposite.MechTypes.Count >= MechComposite.MAX_CHAIN)
                return false;

            bool success = MechBag.TryConsumeMech(toEquip.GetMechType());
            if (!success)
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
            SkillInventory.Add(runSkill);

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

    private int _turn;
    public int Turn => _turn;

    private float _xiuWei;
    public float XiuWei => _xiuWei;

    public void AddXiuWei(int xiuWei = 10)
    {
        _xiuWei += xiuWei;
    }

    public void RemoveXiuWei(int value)
    {
        _xiuWei -= value;
    }

    public void AddHealth(int health)
    {
        Home.SetDHealth(Home.GetDHealth() + health);
    }

    public MingYuan GetMingYuan()
        => Home.MingYuan;

    public void SetDMingYuan(int value)
    {
        Home.MingYuan.SetDiff(value);
    }

    public void ForceDrawSkills(Predicate<SkillEntry> pred = null, WuXing? wuXing = null,
        JingJie? jingJie = null, int count = 1, bool distinct = true, bool consume = true)
        => ForceDrawSkills(new DrawSkillsDetails(pred, wuXing, jingJie, count, distinct, consume));

    public void ForceDrawSkills(DrawSkillsDetails d)
    {
        bool success = SkillPool.TryDrawSkills(out List<RunSkill> skills, d);
        if (!success)
            throw new Exception();

        ForceAddSkills(skills);
    }

    public void ForceDrawSkill(Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
        => ForceDrawSkill(new DrawSkillDetails(pred, wuXing, jingJie));

    public void ForceDrawSkill(DrawSkillDetails d)
    {
        bool success = SkillPool.TryDrawSkill(out RunSkill skill, d);
        if (!success)
            throw new Exception();

        ForceAddSkill(skill);
    }

    public void ForceAddSkills(List<RunSkill> skills)
    {
        foreach(RunSkill skill in skills)
            ForceAddSkill(skill);
    }

    public void ForceAddSkill(AddSkillDetails d)
        => ForceAddSkill(RunSkill.From(d._entry, d._jingJie));

    public void ForceAddSkill(RunSkill skill)
        => SkillInventory.Add(skill);

    public void ForceAddMech([CanBeNull] MechType mechType = null, int count = 1)
        => ForceAddMech(new(mechType, count));
    public void ForceAddMech(AddMechDetails d)
    {
        if (d._mechType == null)
        {
            for (int i = 0; i < d._count; i++)
            {
                MechType mechType = MechType.FromIndex(RandomManager.Range(0, MechType.Length));
                MechBag.AddMech(mechType);
            }
        }
        else
        {
            MechBag.AddMech(d._mechType, d._count);
        }
    }

    public bool CanAffordTech(Address address)
    {
        RunTech runTech = address.Get<RunTech>();
        return runTech.GetCost() <= _xiuWei;
    }

    public bool TrySetDoneTech(Address address)
    {
        if (!CanAffordTech(address))
            return false;

        RunTech runTech = address.Get<RunTech>();
        _xiuWei -= runTech.GetCost();
        TechInventory.SetDone(runTech);
        return true;
    }
}
