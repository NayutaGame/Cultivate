
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLLibrary;
using JetBrains.Annotations;

public class RunEnvironment : Addressable, CLEventListener
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();
    private void Simulate()
    {
        RunEntity away = Away ?? RunEntity.FromJingJieHealth(Home.GetJingJie(), 1000000);
        StageConfig d = new StageConfig(false, false, false, false, Home, away);
        StageEnvironment environment = StageEnvironment.FromConfig(d);
        environment.Execute();
        SimulateResult = environment.Result;
    }

    public event Action ResourceChangedEvent;
    public void ResourceChanged() => ResourceChangedEvent?.Invoke();

    #region Procedures

    public async Task StartRunProcedure(RunDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.START_RUN, d);
    }

    public async Task SetJingJieProcedure(SetJingJieDetails d)
    {
        await _eventDict.SendEvent(CLEventDict.WILL_SET_JINGJIE, d);
        if (d.Cancel)
            return;

        Map.SetJingJie(d.JingJie);
        Home.SetBaseHealth(RunEntity.BaseHealthFromJingJie[d.JingJie]);
        Home.SetJingJie(d.JingJie);
        AudioManager.Play(Encyclopedia.AudioFromJingJie(d.JingJie));

        await _eventDict.SendEvent(CLEventDict.DID_SET_JINGJIE, d);
    }

    public async Task SetMingYuanDiffProcedure()
    {

    }

    public async Task SetMingYuanDMaxProcedure()
    {

    }

    public void SetDMingYuan(int value)
    {
        Home.MingYuan.SetDiff(value);
        ResourceChanged();

        if (GetMingYuan().GetCurr() <= 0)
            CommitDetails = new RunCommitDetails(false);
    }

    #endregion

    public RunConfig _config;
    public RunEntity Home { get; private set; }
    public RunEntity Away { get; set; }
    public Map Map { get; private set; }
    public TechInventory TechInventory { get; private set; }
    public SkillPool SkillPool { get; private set; }
    public SkillInventory Hand { get; private set; }
    public MechBag MechBag { get; private set; }
    public float Gold { get; private set; }
    private CLEventDict _eventDict;

    public StageResult SimulateResult;
    public RunCommitDetails CommitDetails { get; private set; }
    private RunResultPanelDescriptor _runResultPanelDescriptor;

    public MingYuan GetMingYuan()
        => Home.MingYuan;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEnvironment(RunConfig config)
    {
        _accessors = new()
        {
            { "Hero",                  () => Home },
            { "Map",                   () => Map },
            { "TechInventory",         () => TechInventory },
            { "Hand",                  () => Hand },
            { "MechBag",               () => MechBag },
            { "ActivePanel",           GetActivePanel },
        };

        _config = config;

        Home = RunEntity.Default();
        Home.EnvironmentChangedEvent += EnvironmentChanged;
        EnvironmentChangedEvent += Simulate;
        Map = new(config);
        TechInventory = new();
        SkillPool = new();
        Hand = new();
        MechBag = new();
        Gold = 0;
        _eventDict = new();
    }

    public static RunEnvironment FromConfig(RunConfig config)
    {
        return new(config);
    }

    public void Register()
    {
        RegisterCharacterProfile();
        RegisterDifficultyProfile();
        RegisterDesignerConfig();
    }

    private void RegisterCharacterProfile()
    {
        CharacterEntry entry = _config.CharacterProfile.GetEntry();

        foreach (int eventId in entry._eventDescriptorDict.Keys)
        {
            CLEventDescriptor eventDescriptor = entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == CLEventDict.RUN_ENVIRONMENT)
                _eventDict.Register(this, eventDescriptor);
        }
    }

    private void RegisterDifficultyProfile()
    {
    }

    private void RegisterDesignerConfig()
    {
        Dictionary<int, CLEventDescriptor> dict = _config.DesignerConfig._eventDescriptorDict;
        foreach (int eventId in dict.Keys)
        {
            CLEventDescriptor eventDescriptor = dict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == CLEventDict.RUN_ENVIRONMENT)
                _eventDict.Register(this, eventDescriptor);
        }
    }

    public void Unregister()
    {
        UnregisterCharacterProfile();
        UnregisterDifficultyProfile();
        UnregisterDesignerConfig();
    }

    private void UnregisterCharacterProfile()
    {
        CharacterEntry entry = _config.CharacterProfile.GetEntry();

        foreach (int eventId in entry._eventDescriptorDict.Keys)
        {
            CLEventDescriptor eventDescriptor = entry._eventDescriptorDict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == CLEventDict.RUN_ENVIRONMENT)
                _eventDict.Unregister(this, eventDescriptor);
        }
    }

    private void UnregisterDifficultyProfile()
    {
    }

    private void UnregisterDesignerConfig()
    {
        Dictionary<int, CLEventDescriptor> dict = _config.DesignerConfig._eventDescriptorDict;
        foreach (int eventId in dict.Keys)
        {
            CLEventDescriptor eventDescriptor = dict[eventId];
            int senderId = eventDescriptor.ListenerId;

            if (senderId == CLEventDict.RUN_ENVIRONMENT)
                _eventDict.Unregister(this, eventDescriptor);
        }
    }

    public void Combat()
    {
        StageConfig d = new StageConfig(true, true, false, false, Home, Away);
        StageEnvironment environment = StageEnvironment.FromConfig(d);
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
            Hand.Remove(lhs);
            Hand.SetModified(rhs);
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

        Hand.Replace(rhs, newSkill);
        Hand.Remove(lhs);
        EnvironmentChanged();
        return true;
    }

    public bool TryEquipSkill(RunSkill toEquip, SkillSlot slot)
    {
        EmulatedSkill toUnequip = slot.Skill;

        if (toUnequip == null)
            Hand.Remove(toEquip);
        else if (toUnequip is RunSkill runSkill)
            Hand.Replace(toEquip, runSkill);
        else if (toUnequip is MechComposite mechComposite)
        {
            Hand.Remove(toEquip);
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
            Hand.Add(runSkill);
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

    public UnequipResult TryUnequip(SkillSlot slot, object _)
    {
        EmulatedSkill toUnequip = slot.Skill;
        if (toUnequip == null)
            return new(false);

        if (toUnequip is RunSkill runSkill)
        {
            Hand.Add(runSkill);

            slot.Skill = null;
            EnvironmentChanged();

            UnequipResult result = new(true)
            {
                IsRunSkill = true,
                RunSkill = runSkill.Clone()
            };

            return result;
        }
        else if (toUnequip is MechComposite mechComposite)
        {
            foreach (MechType m in mechComposite.MechTypes)
                MechBag.AddMech(m);

            slot.Skill = null;
            EnvironmentChanged();

            UnequipResult result = new(true)
            {
                IsRunSkill = false,
                MechTypes = mechComposite.MechTypes
            };

            return result;
        }

        return new(false);
    }

    public bool TrySwap(SkillSlot fromSlot, SkillSlot toSlot)
    {
        EmulatedSkill temp = fromSlot.Skill;
        fromSlot.Skill = toSlot.Skill;
        toSlot.Skill = temp;
        return true;
    }

    public void SetDGold(int gold = 10)
    {
        Gold += gold;
        ResourceChanged();
    }

    public void SetDDHealth(int dHealth)
    {
        Home.SetDHealth(Home.GetDHealth() + dHealth);
        ResourceChanged();
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
        => Hand.Add(skill);

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
        return runTech.GetCost() <= Gold;
    }

    public bool TrySetDoneTech(Address address)
    {
        if (!CanAffordTech(address))
            return false;

        RunTech runTech = address.Get<RunTech>();
        Gold -= runTech.GetCost();
        TechInventory.SetDone(runTech);
        return true;
    }

    public PanelDescriptor GetActivePanel()
        => _runResultPanelDescriptor ?? Map.CurrentNode?.CurrentPanel;

    public bool TryCommit()
    {
        if (CommitDetails == null)
            return false;

        _runResultPanelDescriptor = new RunResultPanelDescriptor(CommitDetails);
        return true;
    }

    public RunSkill FindSkillInHandWithEntry(SkillEntry entry)
    {
        return Hand.Traversal().FirstObj(s => s.GetEntry() == entry);
    }
}
