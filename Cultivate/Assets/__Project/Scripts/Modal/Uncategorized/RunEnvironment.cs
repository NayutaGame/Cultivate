
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

public class RunEnvironment : Addressable
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();
    private void Simulate()
    {
        RunEntity away = Away ?? RunEntity.FromJingJieHealth(Home.GetJingJie(), 1000000);
        StageConfig d = new StageConfig(false, false, false, false, Home, away);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
        SimulateResult = environment.Result;
    }

    public event Action<JingJie> MapJingJieChangedEvent;
    public void MapJingJieChanged(JingJie jingJie) => MapJingJieChangedEvent?.Invoke(jingJie);
    private void SetHomeJingJieAndBaseHealth(JingJie jingJie)
    {
        Home.SetBaseHealth(RunEntity.BaseHealthFromJingJie[jingJie]);
        Home.SetJingJie(jingJie);
    }

    public RunEntity Home { get; private set; }
    public RunEntity Away { get; set; }

    public Map Map { get; private set; }

    public TechInventory TechInventory { get; private set; }

    public SkillPool SkillPool { get; private set; }
    public SkillInventory SkillInventory { get; private set; }
    public MechBag MechBag { get; private set; }

    public StageEnvironmentResult SimulateResult;

    private RunCommitDetails _commitDetails;

    public RunCommitDetails CommitDetails
    {
        get => _commitDetails;
        set => _commitDetails = value;
    }

    private RunResultPanelDescriptor _runResultPanelDescriptor;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEnvironment(RunConfig runConfig = null)
    {
        _accessors = new()
        {
            { "Hero",                  () => Home },
            { "Map",                   () => Map },
            { "TechInventory",         () => TechInventory },
            { "SkillInventory",        () => SkillInventory },
            { "MechBag",               () => MechBag },
            { "ActivePanel",           GetActivePanel },
        };

        Home = RunEntity.Default();
        Home.EnvironmentChangedEvent += EnvironmentChanged;
        EnvironmentChangedEvent += Simulate;

        Map = new(runConfig);
        Map.JingJieChangedEvent += MapJingJieChanged;
        MapJingJieChangedEvent += SetHomeJingJieAndBaseHealth;

        TechInventory = new();

        SkillInventory = new();
        MechBag = new();

        SkillPool = new();

        _xiuWei = 0;

        runConfig?.Execute(this);
    }

    public static RunEnvironment Default()
        => new();

    public static RunEnvironment FromConfig(RunConfig config)
        => new(config);

    public void Combat()
    {
        StageConfig d = new StageConfig(true, true, false, false, Home, Away);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
    }

    public bool TryMerge(RunSkill lhs, RunSkill rhs)
    {
        if (lhs.GetJingJie() > Home.GetJingJie() && lhs.GetEntry() != rhs.GetEntry())
            return false;

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

        if (GetMingYuan().GetCurr() <= 0)
            _commitDetails = new RunCommitDetails(false);
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

    public PanelDescriptor GetActivePanel()
        => _runResultPanelDescriptor ?? Map.CurrentNode?.CurrentPanel;

    public bool TryCommit()
    {
        if (_commitDetails == null)
            return false;

        _runResultPanelDescriptor = new RunResultPanelDescriptor(_commitDetails);
        return true;
    }
}
