
using System;
using System.Collections.Generic;
using CLLibrary;
using JetBrains.Annotations;
using UnityEngine;

public class RunEnvironment : Addressable, RunEventListener
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    public event Action ResourceChangedEvent;
    public void ResourceChanged() => ResourceChangedEvent?.Invoke();

    #region Memory

    private Dictionary<string, object> Memory;

    public void SetVariable<T>(string key, T value)
        => Memory[key] = value;

    public T GetVariable<T>(string key)
        => (T)Memory[key];

    #endregion

    #region Procedures

    // TODO: cache result
    private void SimulateProcedure()
    {
        PlacementProcedure();
        FormationProcedure();

        StageConfig d = new StageConfig(false, false, false, false, _home, _away);
        StageEnvironment environment = StageEnvironment.FromConfig(d);
        environment.Execute();
        SimulateResult = environment.Result;
    }

    private void PlacementProcedure()
    {
        _home.PlacementProcedure();
        _away.PlacementProcedure();
    }

    private void FormationProcedure()
    {
        _home.FormationProcedure();
        _away.FormationProcedure();
    }

    public void StartRunProcedure(RunDetails d)
    {
        _eventDict.SendEvent(RunEventDict.START_RUN, d);
    }

    public void SetJingJieProcedure(JingJie jingJie)
        => SetJingJieProcedure(new SetJingJieDetails(jingJie));
    private void SetJingJieProcedure(SetJingJieDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_SET_JINGJIE, d);
        if (d.Cancel)
            return;

        Map.SetJingJie(d.JingJie);
        _home.SetBaseHealth(RunEntity.BaseHealthFromJingJie[d.JingJie]);
        _home.SetJingJie(d.JingJie);
        AudioManager.Play(Encyclopedia.AudioFromJingJie(d.JingJie));

        _eventDict.SendEvent(RunEventDict.DID_SET_JINGJIE, d);
    }

    public void SetDMingYuanProcedure(int value)
        => SetDMingYuanProcedure(new SetDMingYuanDetails(value));
    private void SetDMingYuanProcedure(SetDMingYuanDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_SET_D_MINGYUAN, d);

        if (d.Cancel)
            return;

        _home.MingYuan.SetDiff(d.Value);
        ResourceChanged();

        _eventDict.SendEvent(RunEventDict.DID_SET_D_MINGYUAN, d);

        if (GetMingYuan().GetCurr() <= 0)
            CommitDetails = new RunCommitDetails(false);
    }

    public void SetMaxMingYuanProcedure(int value)
        => SetMaxMingYuanProcedure(new SetMaxMingYuanDetails(value));
    private void SetMaxMingYuanProcedure(SetMaxMingYuanDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_SET_MAX_MINGYUAN, d);

        if (d.Cancel)
            return;

        int diff = _home.MingYuan.GetCurr() - d.Value;
        if (diff > 0)
            SetDMingYuanProcedure(diff);

        _home.MingYuan.SetMax(d.Value);

        _eventDict.SendEvent(RunEventDict.DID_SET_MAX_MINGYUAN, d);
    }

    public void DiscoverSkillProcedure(DiscoverSkillDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_DISCOVER_SKILL, d);

        SkillPool.TryDrawSkills(out List<RunSkill> skills, pred: d.Pred, wuXing: d.WuXing, jingJie: d.JingJie , count: d.Count);
        d.Skills.AddRange(skills);

        _eventDict.SendEvent(RunEventDict.DID_DISCOVER_SKILL, d);
    }

    #endregion

    public RunConfig _config;
    private RunEntity _home; public RunEntity Home => _home;
    private RunEntity _away; public RunEntity Away => _away;
    public Map Map { get; private set; }
    public TechInventory TechInventory { get; private set; }
    public SkillPool SkillPool { get; private set; }
    public SkillInventory Hand { get; private set; }
    public MechBag MechBag { get; private set; }
    public float Gold { get; private set; }
    private RunEventDict _eventDict; public RunEventDict EventDict => _eventDict;

    public StageResult SimulateResult;
    public RunCommitDetails CommitDetails { get; private set; }
    private RunResultPanelDescriptor _runResultPanelDescriptor;

    public static RunEnvironment FromConfig(RunConfig config)
        => new(config);

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEnvironment(RunConfig config)
    {
        _accessors = new()
        {
            { "Hero",                  () => _home },
            { "Map",                   () => Map },
            { "TechInventory",         () => TechInventory },
            { "Hand",                  () => Hand },
            { "MechBag",               () => MechBag },
            { "ActivePanel",           GetActivePanel },
        };

        _config = config;

        Memory = new();

        SetHome(RunEntity.Default());
        SetAway(null);

        Map = new();
        TechInventory = new();
        SkillPool = new();
        Hand = new();
        MechBag = new();
        Gold = 0;
        _eventDict = new();

        EnvironmentChangedEvent += SimulateProcedure;
    }

    public MingYuan GetMingYuan()
        => _home.MingYuan;

    public void SetHome(RunEntity home)
    {
        if (_home != null) _home.EnvironmentChangedEvent -= EnvironmentChanged;
        _home = home;
        if (_home != null) _home.EnvironmentChangedEvent += EnvironmentChanged;
    }

    public void SetAway(RunEntity away)
    {
        _awayIsDummy = away == null;
        away ??= RunEntity.FromJingJieHealth(_home.GetJingJie(), 1000000);

        if (_away != null) _away.EnvironmentChangedEvent -= EnvironmentChanged;
        _away = away;
        if (_away != null) _away.EnvironmentChangedEvent += EnvironmentChanged;
    }

    [NonSerialized] private bool _awayIsDummy;
    public bool AwayIsDummy()
        => _awayIsDummy;

    public void Register()
    {
        RegisterCharacterProfile();
        RegisterDifficultyProfile();
        RegisterDesignerConfig();
    }

    private void RegisterCharacterProfile()
    {
        RegisterList(_config.CharacterProfile.GetEntry()._runEventDescriptors);
    }

    private void RegisterDifficultyProfile()
    {
    }

    private void RegisterDesignerConfig()
    {
        RegisterList(_config.DesignerConfig._runEventDescriptors);
    }

    private void RegisterList(RunEventDescriptor[] list)
    {
        list.FilterObj(d => d.ListenerId == RunEventDict.RUN_ENVIRONMENT)
            .Do(e => _eventDict.Register(this, e));
    }

    public void Unregister()
    {
        UnregisterCharacterProfile();
        UnregisterDifficultyProfile();
        UnregisterDesignerConfig();
    }

    private void UnregisterCharacterProfile()
    {
        UnregisterList(_config.CharacterProfile.GetEntry()._runEventDescriptors);
    }

    private void UnregisterDifficultyProfile()
    {
    }

    private void UnregisterDesignerConfig()
    {
        UnregisterList(_config.DesignerConfig._runEventDescriptors);
    }

    private void UnregisterList(RunEventDescriptor[] list)
    {
        list.FilterObj(d => d.ListenerId == RunEventDict.RUN_ENVIRONMENT)
            .Do(e => _eventDict.Unregister(this, e));
    }

    public void Combat()
    {
        StageConfig d = new StageConfig(true, true, false, false, _home, _away);
        StageEnvironment environment = StageEnvironment.FromConfig(d);
        environment.Execute();
    }

    public bool TryMerge(RunSkill lhs, RunSkill rhs)
    {
        if (lhs.GetJingJie() > _home.GetJingJie() && lhs.GetEntry() != rhs.GetEntry())
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

        if (lhs.GetEntry() == rhs.GetEntry() && upgrade && lhs.GetEntry().JingJieContains(lhs.JingJie + 1))
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
        fromSlot.SetSkillWithoutInvokeChange(toSlot.Skill);
        toSlot.SetSkillWithoutInvokeChange(temp);
        EnvironmentChanged();
        return true;
    }

    public void SetDGold(int gold = 10)
    {
        Gold += gold;
        ResourceChanged();
    }

    public void SetDDHealth(int dHealth)
    {
        _home.SetDHealth(_home.GetDHealth() + dHealth);
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
