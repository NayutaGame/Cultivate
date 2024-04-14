
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

    public void StartRunProcedure(RunDetails d)
    {
        bool firstTime = true;
        
        // init map
        {
            // init entity pool
            Map.EntityPool = new();
            Map.EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal());
            Map.EntityPool.Shuffle();
            
            // init adventure pool
            Map.AdventurePool = new();
            Map.AdventurePool.Populate(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool));
            Map.AdventurePool.Shuffle();
            
            // init skill pool
            4.Do(i => SkillPool.Populate(Encyclopedia.SkillCategory.Traversal.FilterObj(e => e.WithinPool)));
            
            // init drawers
            Map.StepDescriptors = new StepDescriptor[]
            {
                firstTime ? new DirectStepDescriptor("初入蓬莱") : new AdventureStepDescriptor(),
                new BattleStepDescriptor(0, 3, 4),
                new AdventureStepDescriptor(),
                new RestStepDescriptor(),
                new BattleStepDescriptor(1, 4, 5),
                new AscensionStepDescriptor(),
                
                new BattleStepDescriptor(2, 5, 6),
                new AdventureStepDescriptor(),
                new RestStepDescriptor(),
                new BattleStepDescriptor(3, 6, 7),
                new AdventureStepDescriptor(),
                new RestStepDescriptor(),
                new BattleStepDescriptor(4, 7, 8),
                new AscensionStepDescriptor(),
                
                new BattleStepDescriptor(5, 8, 8),
                new AdventureStepDescriptor(),
                new RestStepDescriptor(),
                new BattleStepDescriptor(5, 8, 9),
                new AdventureStepDescriptor(),
                new BattleStepDescriptor(6, 9, 9),
                new AdventureStepDescriptor(),
                new RestStepDescriptor(),
                new BattleStepDescriptor(7, 9, 10),
                new AscensionStepDescriptor(),
                
                new BattleStepDescriptor(8, 10, 10),
                new AdventureStepDescriptor(),
                new BattleStepDescriptor(8, 10, 11),
                new AdventureStepDescriptor(),
                new RestStepDescriptor(),
                new BattleStepDescriptor(9, 11, 11),
                new AdventureStepDescriptor(),
                new BattleStepDescriptor(9, 11, 12),
                new AdventureStepDescriptor(),
                new RestStepDescriptor(),
                new BattleStepDescriptor(10, 12, 12),
                new AscensionStepDescriptor(),
                
                new BattleStepDescriptor(11, 12, 12),
                new AdventureStepDescriptor(),
                new BattleStepDescriptor(11, 12, 12),
                new AdventureStepDescriptor(),
                new BattleStepDescriptor(11, 12, 12),
                new RestStepDescriptor(),
                new BattleStepDescriptor(12, 12, 12),
                new AdventureStepDescriptor(),
                new BattleStepDescriptor(12, 12, 12),
                new AdventureStepDescriptor(),
                new BattleStepDescriptor(12, 12, 12),
                new RestStepDescriptor(),
                new BattleStepDescriptor(13, 12, 12),
                new SuccessStepDescriptor(),
            };
        }

        {
            SetJingJieProcedure(JingJie.LianQi);
            SetStepProcedure(0);
            _home.SetSlotCount(3);
        }

        {
            // init player start condition
            SetDGold(50);
            if (!firstTime)
                ForceDrawSkills(jingJie: JingJie.LianQi, count: 5);
            // ForceDrawSkills(jingJie: JingJie.HuaShen, count: 20);
        }
        
        _eventDict.SendEvent(RunEventDict.START_RUN, d);
    }

    public void NextJingJieProcedure()
        => SetJingJieProcedure(Map.JingJie + 1);
    public void SetJingJieProcedure(JingJie toJingJie)
        => SetJingJieProcedure(new SetJingJieDetails(Map.JingJie, toJingJie));
    private void SetJingJieProcedure(SetJingJieDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WIL_SET_JINGJIE, d);
        if (d.Cancel)
            return;

        Map.SetJingJie(d.ToJingJie);
        
        // move to ascension
        _home.SetBaseHealth(RunEntity.BaseHealthFromJingJie[d.ToJingJie]);
        
        _home.SetJingJie(d.ToJingJie);
        AudioManager.Play(Encyclopedia.AudioFromJingJie(d.ToJingJie));

        _eventDict.SendEvent(RunEventDict.DID_SET_JINGJIE, d);
    }

    private void SetStepProcedure(int step)
    {
        Map.Step = step;
        Map.DrawNode();
        Map.CurrStepItem.ToChoose();
        Map.Choosing = true;
    }

    private void NextStepProcedure()
        => SetStepProcedure(Map.Step + 1);

    public PanelDescriptor MakeChoiceProcedure(RunNode runNode)
    {
        Map.Choice = Map.CurrStepItem.IndexOf(runNode);
        Map.CurrStepItem.MakeChoice(Map.Choice);
        Map.Choosing = false;
        Map.CreateEntry();
        return Map.CurrNode.Panel;
    }

    public PanelDescriptor ReceiveSignalProcedure(Signal signal)
    {
        PanelDescriptor panelDescriptor = Map.CurrNode.Panel.ReceiveSignal(signal);
        if (panelDescriptor != null)
        {
            Map.CurrNode.Panel = panelDescriptor;
        }
        else
        {
            TryFinishStep();
        }

        bool commit = TryCommit();
        if (!commit)
            return panelDescriptor;

        panelDescriptor = GetActivePanel();
        Map.CurrNode.Panel = panelDescriptor;
        return panelDescriptor;
    }

    private void TryFinishStep()
    {
        if (Map.Choosing)
            return;

        Map.CurrNode.Finish();

        bool isLastStep = Map.IsLastStep();
        if (!isLastStep)
        {
            NextStepProcedure();
            return;
        }

        CommitRun();
    }
    
    private void CommitRun() { }

    public void Combat()
    {
        StageEnvironment.Combat(StageConfig.ForCombat(_home, _away, _config));
    }

    // TODO: cache result
    private void SimulateProcedure()
    {
        PlacementProcedure();
        FormationProcedure();

        SimulateResult = StageEnvironment.CalcSimulateResult(StageConfig.ForSimulate(_home, _away, _config));
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
            Result.State = RunResult.RunResultState.Defeat;
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
    public RunResult Result { get; }
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

        Result = new RunResult();

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
        RegisterList(_config.CharacterProfile.GetEntry()._runEventDescriptors);

        DifficultyEntry difficultyEntry = _config.DifficultyProfile.GetEntry();
        RegisterList(difficultyEntry._runEventDescriptors);
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
            RegisterList(additionalDifficultyEntry._runEventDescriptors);

        RegisterList(_config.DesignerConfig._runEventDescriptors);
    }

    private void RegisterList(RunEventDescriptor[] list)
    {
        list.FilterObj(d => d.ListenerId == RunEventDict.RUN_ENVIRONMENT)
            .Do(e => _eventDict.Register(this, e));
    }

    public void Unregister()
    {
        UnregisterList(_config.CharacterProfile.GetEntry()._runEventDescriptors);

        DifficultyEntry difficultyEntry = _config.DifficultyProfile.GetEntry();
        UnregisterList(difficultyEntry._runEventDescriptors);
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
            UnregisterList(additionalDifficultyEntry._runEventDescriptors);

        UnregisterList(_config.DesignerConfig._runEventDescriptors);
    }

    private void UnregisterList(RunEventDescriptor[] list)
    {
        list.FilterObj(d => d.ListenerId == RunEventDict.RUN_ENVIRONMENT)
            .Do(e => _eventDict.Unregister(this, e));
    }

    public bool MergeProcedure(RunSkill lhs, RunSkill rhs)
    {
        SkillEntry lEntry = lhs.GetEntry();
        SkillEntry rEntry = rhs.GetEntry();
        JingJie lJingJie = lhs.GetJingJie();
        JingJie rJingJie = rhs.GetJingJie();
        WuXing? lWuXing = lEntry.WuXing;
        WuXing? rWuXing = rEntry.WuXing;

        if (rJingJie > _home.GetJingJie() || lJingJie > _home.GetJingJie())
            return false;
        
        // Congruent
        if (lEntry == rEntry && lJingJie == rJingJie && lJingJie < rEntry.HighestJingJie)
        {
            rhs.JingJie = (rJingJie + 1).ClampUpper(rEntry.HighestJingJie);
            Hand.Remove(lhs);
            Hand.SetModified(rhs);
            EnvironmentChanged();
            return true;
        }
        
        // Same Name
        if (lEntry == rEntry && Mathf.Max(lJingJie, rJingJie) < rEntry.HighestJingJie)
        {
            rhs.JingJie = (Mathf.Max(lJingJie, rJingJie) + 1).ClampUpper(rEntry.HighestJingJie);
            Hand.Remove(lhs);
            Hand.SetModified(rhs);
            EnvironmentChanged();
            return true;
        }
        
        // Formula
        if (false && lJingJie == rJingJie)
        {
            return false;
        }
        
        // Same WuXing
        if (lWuXing == rWuXing && lJingJie == rJingJie && rJingJie < rEntry.HighestJingJie)
        {
            SkillPool.TryDrawSkill(out RunSkill newSkill,
                pred: skillEntry => skillEntry != lEntry && skillEntry != rEntry,
                wuXing: rWuXing,
                jingJie: (rJingJie + 1).ClampUpper(rEntry.HighestJingJie));
            Hand.Remove(lhs);
            Hand.Replace(rhs, newSkill);
            EnvironmentChanged();
            return true;
        }
        
        // XiangSheng WuXing
        if (WuXing.XiangSheng(lWuXing, rWuXing) && lJingJie == rJingJie && rJingJie < rEntry.HighestJingJie)
        {
            SkillPool.TryDrawSkill(out RunSkill newSkill,
                wuXing: WuXing.XiangShengNext(lWuXing, rWuXing).Value,
                jingJie: (rJingJie + 1).ClampUpper(rEntry.HighestJingJie));
            Hand.Remove(lhs);
            Hand.Replace(rhs, newSkill);
            EnvironmentChanged();
            return true;
        }
        
        // Same JingJie
        if (lJingJie == rJingJie && rJingJie < rEntry.HighestJingJie)
        {
            SkillPool.TryDrawSkill(out RunSkill newSkill,
                pred: skillEntry => skillEntry.WuXing.HasValue && skillEntry.WuXing != lWuXing && skillEntry.WuXing != rWuXing,
                jingJie: (rJingJie + 1).ClampUpper(rEntry.HighestJingJie));
            Hand.Remove(lhs);
            Hand.Replace(rhs, newSkill);
            EnvironmentChanged();
            return true;
        }
        
        return false;
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
        => ForceAddSkill(RunSkill.FromEntry(d._entry, d._jingJie));

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
        => _runResultPanelDescriptor ?? Map.CurrNode?.Panel;

    private bool TryCommit()
    {
        if (Result.State == RunResult.RunResultState.Unfinished)
            return false;

        _runResultPanelDescriptor = new RunResultPanelDescriptor(Result);
        return true;
    }

    public RunSkill FindSkillInHandWithEntry(SkillEntry entry)
    {
        return Hand.Traversal().FirstObj(s => s.GetEntry() == entry);
    }
}
