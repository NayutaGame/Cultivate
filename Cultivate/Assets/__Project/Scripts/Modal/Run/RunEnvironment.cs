
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class RunEnvironment : Addressable, RunEventListener
{
    #region Memory

    private Dictionary<string, object> _memory;

    public void SetVariable<T>(string key, T value)
        => _memory[key] = value;

    public T GetVariable<T>(string key)
        => (T)_memory[key];

    #endregion

    #region Neurons

    public Neuron BattleChangedNeuron = new();
    private void BattleEnvironmentUpdateProcedure()
    {
        EnvironmentUpdateDetails d = new();
        _eventDict.SendEvent(RunEventDict.WIL_UPDATE, d);
        SimulateProcedure();
        _eventDict.SendEvent(RunEventDict.DID_UPDATE, d);
    }
    
    #endregion

    #region Procedures

    public void StartRunProcedure(RunDetails d)
    {
        bool firstTime = AppManager.Instance.ShowTutorial;
        
        Map.InitEntityPool();
        Map.InitAdventurePool();
        Map.InsertedAdventurePool = new();
        
        InitSkillPool();

        InitMapFromJingJie(firstTime, AppManager.Instance.SimulatingJingJie);
        
        _eventDict.SendEvent(RunEventDict.START_RUN, d);
    }

    private void InitSkillPool()
    {
        4.Do(_ => SkillPool.Populate(Encyclopedia.SkillCategory.Traversal.FilterObj(e => e.WithinPool)));
    }

    private void InitMap(bool firstTime)
    {
        Map.StepDescriptors = new StepDescriptor[]
        {
            firstTime ? new DirectStepDescriptor(0, "初入蓬莱") : new AdventureStepDescriptor(0),
            new BattleStepDescriptor(0, 3, 4),
            new AdventureStepDescriptor(0),
            new RestStepDescriptor(0),
            new BattleStepDescriptor(1, 4, 5),
            new AscensionStepDescriptor(0),
            
            new BattleStepDescriptor(2, 5, 6),
            new AdventureStepDescriptor(2),
            new ShopStepDescriptor(2),
            new BattleStepDescriptor(3, 6, 7),
            new AdventureStepDescriptor(3),
            new RestStepDescriptor(3),
            new BattleStepDescriptor(4, 7, 8),
            new AscensionStepDescriptor(4),
            
            new BattleStepDescriptor(5, 8, 8),
            new AdventureStepDescriptor(5),
            new ShopStepDescriptor(5),
            new BattleStepDescriptor(5, 8, 9),
            new AdventureStepDescriptor(5),
            new BattleStepDescriptor(6, 9, 9),
            new AdventureStepDescriptor(6),
            new RestStepDescriptor(6),
            new BattleStepDescriptor(7, 9, 10),
            new AscensionStepDescriptor(7),
            
            new BattleStepDescriptor(8, 10, 10),
            new AdventureStepDescriptor(8),
            new BattleStepDescriptor(8, 10, 11),
            new AdventureStepDescriptor(8),
            new ShopStepDescriptor(8),
            new BattleStepDescriptor(9, 11, 11),
            new AdventureStepDescriptor(9),
            new BattleStepDescriptor(9, 11, 12),
            new AdventureStepDescriptor(9),
            new RestStepDescriptor(9),
            new BattleStepDescriptor(10, 12, 12),
            new AscensionStepDescriptor(10),
            
            new BattleStepDescriptor(11, 12, 12),
            new AdventureStepDescriptor(11),
            new BattleStepDescriptor(11, 12, 12),
            new AdventureStepDescriptor(11),
            new BattleStepDescriptor(11, 12, 12),
            new RestStepDescriptor(11),
            new BattleStepDescriptor(12, 12, 12),
            new AdventureStepDescriptor(12),
            new BattleStepDescriptor(12, 12, 12),
            new AdventureStepDescriptor(12),
            new BattleStepDescriptor(12, 12, 12),
            new RestStepDescriptor(12),
            new ShopStepDescriptor(12),
            new BattleStepDescriptor(13, 12, 12),
            new SuccessStepDescriptor(13),
        };
        
        SetJingJieProcedure(JingJie.LianQi);
        SetStepProcedure(0);
        _home.SetSlotCount(3);
        
        SetDGoldProcedure(50);
        if (!firstTime)
            DrawSkillsProcedure(new(jingJie: JingJie.LianQi, count: 5));
    }

    private void InitMapFromJingJie(bool firstTime, JingJie jingJie)
    {
        switch (jingJie)
        {
            case 0:
                Map.StepDescriptors = new StepDescriptor[]
                {
                    // new DirectStepDescriptor(0, "快速结算"),
                    firstTime ? new DirectStepDescriptor(0, "初入蓬莱") : new AdventureStepDescriptor(0),
                    new BattleStepDescriptor(0, 3, 4),
                    new AdventureStepDescriptor(0),
                    new RestStepDescriptor(0),
                    new BattleStepDescriptor(1, 4, 5),
                    new AscensionStepDescriptor(0),
                    
                    new BattleStepDescriptor(2, 5, 6),
                    firstTime ? new DirectStepDescriptor(2, "同境界合成教学") : new AdventureStepDescriptor(2),
                    new ShopStepDescriptor(2),
                    new BattleStepDescriptor(3, 6, 7),
                    new AdventureStepDescriptor(3),
                    new RestStepDescriptor(3),
                    new BattleStepDescriptor(4, 7, 8),
                    new AscensionStepDescriptor(4),
                    
                    new BattleStepDescriptor(5, 8, 8),
                    new AdventureStepDescriptor(5),
                    new ShopStepDescriptor(5),
                    new BattleStepDescriptor(5, 8, 9),
                    new AdventureStepDescriptor(5),
                    new BattleStepDescriptor(6, 9, 9),
                    new AdventureStepDescriptor(6),
                    new RestStepDescriptor(6),
                    new BattleStepDescriptor(7, 9, 10),
                    new AscensionStepDescriptor(7),
                    
                    new BattleStepDescriptor(8, 10, 10),
                    new AdventureStepDescriptor(8),
                    new BattleStepDescriptor(8, 10, 11),
                    new AdventureStepDescriptor(8),
                    new ShopStepDescriptor(8),
                    new BattleStepDescriptor(9, 11, 11),
                    new AdventureStepDescriptor(9),
                    new BattleStepDescriptor(9, 11, 12),
                    new AdventureStepDescriptor(9),
                    new RestStepDescriptor(9),
                    new BattleStepDescriptor(10, 12, 12),
                    new AscensionStepDescriptor(10),
                    
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new RestStepDescriptor(11),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new RestStepDescriptor(12),
                    new ShopStepDescriptor(12),
                    new BattleStepDescriptor(13, 12, 12),
                    new SuccessStepDescriptor(13),
                };
        
                SetJingJieProcedure(JingJie.LianQi);
                SetStepProcedure(0);
                _home.SetSlotCount(3);
        
                SetDGoldProcedure(0);
                DrawSkillsProcedure(new(jingJie: JingJie.LianQi, count: 5));
                break;
            case 1:
                Map.StepDescriptors = new StepDescriptor[]
                {
                    new BattleStepDescriptor(2, 5, 6),
                    new AdventureStepDescriptor(2),
                    new ShopStepDescriptor(2),
                    new BattleStepDescriptor(3, 6, 7),
                    new AdventureStepDescriptor(3),
                    new RestStepDescriptor(3),
                    new BattleStepDescriptor(4, 7, 8),
                    new AscensionStepDescriptor(4),
                    
                    new BattleStepDescriptor(5, 8, 8),
                    new AdventureStepDescriptor(5),
                    new ShopStepDescriptor(5),
                    new BattleStepDescriptor(5, 8, 9),
                    new AdventureStepDescriptor(5),
                    new BattleStepDescriptor(6, 9, 9),
                    new AdventureStepDescriptor(6),
                    new RestStepDescriptor(6),
                    new BattleStepDescriptor(7, 9, 10),
                    new AscensionStepDescriptor(7),
                    
                    new BattleStepDescriptor(8, 10, 10),
                    new AdventureStepDescriptor(8),
                    new BattleStepDescriptor(8, 10, 11),
                    new AdventureStepDescriptor(8),
                    new ShopStepDescriptor(8),
                    new BattleStepDescriptor(9, 11, 11),
                    new AdventureStepDescriptor(9),
                    new BattleStepDescriptor(9, 11, 12),
                    new AdventureStepDescriptor(9),
                    new RestStepDescriptor(9),
                    new BattleStepDescriptor(10, 12, 12),
                    new AscensionStepDescriptor(10),
                    
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new RestStepDescriptor(11),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new RestStepDescriptor(12),
                    new ShopStepDescriptor(12),
                    new BattleStepDescriptor(13, 12, 12),
                    new SuccessStepDescriptor(13),
                };
        
                SetJingJieProcedure(JingJie.ZhuJi);
                SetStepProcedure(0);
                _home.SetSlotCount(5);
        
                SetDGoldProcedure(5);
                DrawSkillsProcedure(new(jingJie: JingJie.LianQi, count: 7));
                break;
            case 2:
                Map.StepDescriptors = new StepDescriptor[]
                {
                    new BattleStepDescriptor(5, 8, 8),
                    new AdventureStepDescriptor(5),
                    new ShopStepDescriptor(5),
                    new BattleStepDescriptor(5, 8, 9),
                    new AdventureStepDescriptor(5),
                    new BattleStepDescriptor(6, 9, 9),
                    new AdventureStepDescriptor(6),
                    new RestStepDescriptor(6),
                    new BattleStepDescriptor(7, 9, 10),
                    new AscensionStepDescriptor(7),
                    
                    new BattleStepDescriptor(8, 10, 10),
                    new AdventureStepDescriptor(8),
                    new BattleStepDescriptor(8, 10, 11),
                    new AdventureStepDescriptor(8),
                    new ShopStepDescriptor(8),
                    new BattleStepDescriptor(9, 11, 11),
                    new AdventureStepDescriptor(9),
                    new BattleStepDescriptor(9, 11, 12),
                    new AdventureStepDescriptor(9),
                    new RestStepDescriptor(9),
                    new BattleStepDescriptor(10, 12, 12),
                    new AscensionStepDescriptor(10),
                    
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new RestStepDescriptor(11),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new RestStepDescriptor(12),
                    new ShopStepDescriptor(12),
                    new BattleStepDescriptor(13, 12, 12),
                    new SuccessStepDescriptor(13),
                };
        
                SetJingJieProcedure(JingJie.JinDan);
                SetStepProcedure(0);
                _home.SetSlotCount(8);
        
                SetDGoldProcedure(17);
                DrawSkillsProcedure(new(jingJie: JingJie.LianQi, count: 13));
                break;
            case 3:
                Map.StepDescriptors = new StepDescriptor[]
                {
                    new BattleStepDescriptor(8, 10, 10),
                    new AdventureStepDescriptor(8),
                    new BattleStepDescriptor(8, 10, 11),
                    new AdventureStepDescriptor(8),
                    new ShopStepDescriptor(8),
                    new BattleStepDescriptor(9, 11, 11),
                    new AdventureStepDescriptor(9),
                    new BattleStepDescriptor(9, 11, 12),
                    new AdventureStepDescriptor(9),
                    new RestStepDescriptor(9),
                    new BattleStepDescriptor(10, 12, 12),
                    new AscensionStepDescriptor(10),
                    
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new RestStepDescriptor(11),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new RestStepDescriptor(12),
                    new ShopStepDescriptor(12),
                    new BattleStepDescriptor(13, 12, 12),
                    new SuccessStepDescriptor(13),
                };
        
                SetJingJieProcedure(JingJie.YuanYing);
                SetStepProcedure(0);
                _home.SetSlotCount(10);
        
                SetDGoldProcedure(49);
                DrawSkillsProcedure(new(jingJie: JingJie.ZhuJi, count: 15));
                break;
            case 4:
                Map.StepDescriptors = new StepDescriptor[]
                {
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new AdventureStepDescriptor(11),
                    new BattleStepDescriptor(11, 12, 12),
                    new RestStepDescriptor(11),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new AdventureStepDescriptor(12),
                    new BattleStepDescriptor(12, 12, 12),
                    new RestStepDescriptor(12),
                    new ShopStepDescriptor(12),
                    new BattleStepDescriptor(13, 12, 12),
                    new SuccessStepDescriptor(13),
                };
        
                SetJingJieProcedure(JingJie.HuaShen);
                SetStepProcedure(0);
                _home.SetSlotCount(12);
        
                SetDGoldProcedure(129);
                DrawSkillsProcedure(new(jingJie: JingJie.JinDan, count: 17));
                break;
            case 5:
                Map.StepDescriptors = new StepDescriptor[]
                {
                    new BattleStepDescriptor(13, 12, 12),
                    new SuccessStepDescriptor(13),
                };
        
                SetJingJieProcedure(JingJie.HuaShen);
                SetStepProcedure(0);
                _home.SetSlotCount(12);
        
                SetDGoldProcedure(289);
                DrawSkillsProcedure(new(jingJie: JingJie.JinDan, count: 41));
                break;
        }
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
        
        // move to ascension procedure
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
        if (Map.CurrNode == null)
            return null;
        
        Guide guide = Map.CurrNode.Panel.GetGuideDescriptor();
        if (guide != null)
        {
            bool blocksSignal = guide.ReceiveSignal(Map.CurrNode.Panel, signal);
            if (blocksSignal)
                return Map.CurrNode.Panel;
        }
        
        PanelDescriptor panelDescriptor = Map.CurrNode.Panel.ReceiveSignal(signal);
        bool runIsUnfinished = Result.State == RunResult.RunResultState.Unfinished;
        if (runIsUnfinished)
        {
            if (panelDescriptor != null) // descriptor of panel descriptor
            {
                Map.CurrNode.Panel = panelDescriptor;
            }
            else
            {
                TryFinishStep();
            }
            
            return panelDescriptor;
        }

        _runResultPanelDescriptor = new RunResultPanelDescriptor(Result);
        Map.CurrNode.Panel = _runResultPanelDescriptor;
        return _runResultPanelDescriptor;
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
        
        CommitRunProcedure(RunResult.RunResultState.Victory);
    }

    public void CommitRunProcedure(RunResult.RunResultState state)
    {
        Result.State = state;
    }

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
        _eventDict.SendEvent(RunEventDict.WIL_SET_D_MINGYUAN, d);

        if (d.Cancel)
            return;

        _home.MingYuan.Curr += d.Value;
        _eventDict.SendEvent(RunEventDict.DID_SET_D_MINGYUAN, d);

        // register this as a defeat check
        if (GetMingYuan().Curr <= 0)
            CommitRunProcedure(RunResult.RunResultState.Defeat);
    }

    public void SetDGoldProcedure(int value)
        => SetDGoldProcedure(new SetDGoldDetails(value));
    private void SetDGoldProcedure(SetDGoldDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_SET_D_GOLD, d);

        if (d.Cancel)
            return;

        _gold.Curr += d.Value;
        _eventDict.SendEvent(RunEventDict.DID_SET_D_GOLD, d);
    }

    public void SetDDHealthProcedure(int value)
        => SetDDHealthProcedure(new SetDDHealthDetails(value));
    private void SetDDHealthProcedure(SetDDHealthDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_SET_DDHEALTH, d);

        if (d.Cancel)
            return;

        _home.SetDHealth(_home.GetDHealth() + d.Value);
        _eventDict.SendEvent(RunEventDict.DID_SET_DDHEALTH, d);
    }

    public void SetMaxMingYuanProcedure(int value)
        => SetMaxMingYuanProcedure(new SetMaxMingYuanDetails(value));
    private void SetMaxMingYuanProcedure(SetMaxMingYuanDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_SET_MAX_MINGYUAN, d);

        if (d.Cancel)
            return;

        int diff = _home.MingYuan.Curr - d.Value;
        if (diff > 0)
            SetDMingYuanProcedure(diff);

        _home.MingYuan.UpperBound = d.Value;

        _eventDict.SendEvent(RunEventDict.DID_SET_MAX_MINGYUAN, d);
    }

    public void DiscoverSkillProcedure(DiscoverSkillDetails d)
    {
        _eventDict.SendEvent(RunEventDict.WILL_DISCOVER_SKILL, d);

        List<SkillEntry> entries = DrawSkills(d.Descriptor);
        d.Skills.AddRange(entries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, d.PreferredJingJie)));

        _eventDict.SendEvent(RunEventDict.DID_DISCOVER_SKILL, d);
    }

    #endregion

    private RunConfig _config;
    private RunEntity _home; public RunEntity Home => _home;
    private RunEntity _away; public RunEntity Away => _away;
    public Map Map { get; private set; }
    public TechInventory TechInventory { get; private set; }
    public Pool<SkillEntry> SkillPool;
    public SkillInventory Hand { get; private set; }
    private BoundedInt _gold;
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
            { "Home",                  () => _home },
            { "Map",                   () => Map },
            { "TechInventory",         () => TechInventory },
            { "Hand",                  () => Hand },
            { "ActivePanel",           GetActivePanel },
        };

        _gold = new(0);
        _config = config;

        _memory = new();

        SetHome(RunEntity.Default());
        SetAway(null);

        Map = new();
        TechInventory = new();
        SkillPool = new();
        Hand = new();
        _eventDict = new();

        Result = new RunResult();

        BattleChangedNeuron.Add(BattleEnvironmentUpdateProcedure);
    }

    public BoundedInt GetGold()
        => _gold;

    public MingYuan GetMingYuan()
        => _home.MingYuan;

    public void SetHome(RunEntity home)
    {
        _home?.EnvironmentChangedNeuron.Remove(BattleChangedNeuron);
        _home = home;
        _home?.EnvironmentChangedNeuron.Add(BattleChangedNeuron);
    }

    public void SetAway(RunEntity away)
    {
        _awayIsDummy = away == null;
        away ??= RunEntity.FromJingJieHealth(_home.GetJingJie(), 1000000);
        
        _away?.EnvironmentChangedNeuron.Remove(BattleChangedNeuron);
        _away = away;
        _away?.EnvironmentChangedNeuron.Add(BattleChangedNeuron);
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

    public MergePreresult GetMergePreresult(RunSkill lhs, RunSkill rhs)
    {
        JingJie playerJingJie = _home.GetJingJie();
        
        var tuple = MergePreresult.MergeRules.FirstObj(tuple => tuple.Item1(lhs, rhs, playerJingJie));
        if (tuple != null)
            return tuple.Item2(lhs, rhs, playerJingJie);

        return MergePreresult.FromDefault(lhs, rhs, playerJingJie);
    }

    public bool MergeProcedure(RunSkill lhs, RunSkill rhs)
    {
        MergeDetails d = new MergeDetails(lhs, rhs);
        
        _eventDict.SendEvent(RunEventDict.WIL_MERGE, d);

        if (d.Cancel)
            return false;

        bool success = InterMergeProcedure(d);
        if (!success)
            return false;
        
        _eventDict.SendEvent(RunEventDict.DID_MERGE, d);
        return true;
    }

    private bool InterMergeProcedure(MergeDetails d)
    {
        RunSkill lhs = d.Lhs;
        RunSkill rhs = d.Rhs;
        
        if (lhs.Borrowed || rhs.Borrowed)
            return false;
        
        SkillEntry lEntry = lhs.GetEntry();
        SkillEntry rEntry = rhs.GetEntry();
        JingJie lJingJie = lhs.GetJingJie();
        JingJie rJingJie = rhs.GetJingJie();
        WuXing? lWuXing = lEntry.WuXing;
        WuXing? rWuXing = rEntry.WuXing;
        JingJie playerJingJie = _home.GetJingJie();
        DeckIndex rhsDeckIndex = new DeckIndex(false, Hand.IndexOf(rhs));
        
        if (MergePreresult.IsCongruent(lhs, rhs, playerJingJie))
        {
            rhs.JingJie = (rJingJie + 2).ClampUpper(rEntry.HighestJingJie);
            Hand.Remove(lhs);
            Hand.SetModified(rhs);
            BattleChangedNeuron.Invoke();
            return true;
        }
        
        if (MergePreresult.IsSameName(lhs, rhs, playerJingJie))
        {
            rhs.JingJie = (Mathf.Max(lJingJie, rJingJie) + 1).ClampUpper(rEntry.HighestJingJie);
            Hand.Remove(lhs);
            Hand.SetModified(rhs);
            BattleChangedNeuron.Invoke();
            return true;
        }

        bool valid = rhs.GetJingJie() <= playerJingJie && lhs.GetJingJie() <= playerJingJie;
        if (!valid)
            return false;
        
        if (MergePreresult.IsSameWuXing(lhs, rhs, playerJingJie))
        {
            DrawSkillProcedure(new(
                    pred: skillEntry => skillEntry != lEntry && skillEntry != rEntry,
                    wuXing: rWuXing,
                    jingJie: rJingJie + 1),
                preferredDeckIndex: rhsDeckIndex);
            Hand.Remove(lhs);
            BattleChangedNeuron.Invoke();
            return true;
        }
        
        if (MergePreresult.IsXiangShengWuXing(lhs, rhs, playerJingJie))
        {
            DrawSkillProcedure(new(
                    wuXing: WuXing.XiangShengNext(lWuXing, rWuXing).Value,
                    jingJie: rJingJie + 1),
                preferredDeckIndex: rhsDeckIndex);
            Hand.Remove(lhs);
            BattleChangedNeuron.Invoke();
            return true;
        }
        
        if (MergePreresult.IsSameJingJie(lhs, rhs, playerJingJie))
        {
            DrawSkillProcedure(new(
                    pred: skillEntry => skillEntry.WuXing.HasValue && skillEntry.WuXing != lWuXing &&
                                        skillEntry.WuXing != rWuXing,
                    jingJie: rJingJie + 1),
                preferredDeckIndex: rhsDeckIndex);
            Hand.Remove(lhs);
            BattleChangedNeuron.Invoke();
            return true;
        }
        
        if (MergePreresult.IsHuaShenReroll(lhs, rhs, playerJingJie))
        {
            DrawSkillProcedure(new(
                    pred: skillEntry => skillEntry.WuXing.HasValue && skillEntry.WuXing != lWuXing &&
                                        skillEntry.WuXing != rWuXing,
                    jingJie: rJingJie),
                preferredDeckIndex: rhsDeckIndex);
            Hand.Remove(lhs);
            BattleChangedNeuron.Invoke();
            return true;
        }
        
        return false;
    }

    public bool EquipProcedure(out bool isReplace, RunSkill toEquip, SkillSlot slot)
    {
        RunSkill toUnequip = slot.Skill;

        isReplace = false;

        if (toUnequip == null)
            Hand.Remove(toEquip);
        else if (toUnequip is RunSkill runSkill)
        {
            isReplace = true;
            Hand.Replace(toEquip, runSkill);
        }

        slot.Skill = toEquip;
        return true;
    }

    public UnequipResult UnequipProcedure(SkillSlot slot, object _)
    {
        RunSkill toUnequip = slot.Skill;
        if (toUnequip == null)
            return new(false);

        if (toUnequip is RunSkill runSkill)
        {
            Hand.Add(runSkill);

            slot.Skill = null;

            UnequipResult result = new(true)
            {
                RunSkill = runSkill.Clone()
            };

            return result;
        }

        return new(false);
    }

    public bool SwapProcedure(out bool isReplace, SkillSlot fromSlot, SkillSlot toSlot)
    {
        isReplace = toSlot.Skill != null;
        RunSkill temp = fromSlot.Skill;
        fromSlot.Skill = toSlot.Skill;
        toSlot.Skill = temp;
        return true;
    }

    public bool CanAffordTech(Address address)
    {
        RunTech runTech = address.Get<RunTech>();
        return runTech.GetCost() <= GetGold().Curr;
    }

    public bool TrySetDoneTech(Address address)
    {
        if (!CanAffordTech(address))
            return false;

        RunTech runTech = address.Get<RunTech>();
        GetGold().Curr -= runTech.GetCost();
        TechInventory.SetDone(runTech);
        return true;
    }

    public PanelDescriptor GetActivePanel()
        => _runResultPanelDescriptor ?? Map.CurrNode?.Panel;

    #region Skill
    
    public IEnumerable<DeckIndex> TraversalDeckIndices(bool excludingField = false, bool excludingHand = false)
    {
        if (!excludingField)
            foreach (var slot in RunManager.Instance.Environment.Home.TraversalCurrentSlots())
                yield return new DeckIndex(true, slot.Index);
        if (!excludingHand)
            for (int i = 0; i < RunManager.Instance.Environment.Hand.Count(); i++)
                yield return new DeckIndex(false, i);
    }

    public RunSkill GetSkillAtDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            return Home.GetSlot(deckIndex.Index).Skill;
        else
            return Hand[deckIndex.Index];
    }

    public bool FindDeckIndex(out DeckIndex result, SkillEntryDescriptor d,
        bool excludingField = false, bool excludingHand = false, DeckIndex[] omit = null)
    {
        omit ??= Array.Empty<DeckIndex>();
        
        result = default;
        
        foreach (DeckIndex deckIndex in TraversalDeckIndices(excludingField, excludingHand))
        {
            if (omit.Contains(deckIndex))
                continue;
            RunSkill skill = GetSkillAtDeckIndex(deckIndex);
            if (skill != null && d.Contains(skill.GetEntry()))
            {
                result = deckIndex;
                return true;
            }
        }

        return false;
    }

    public void ClearDeck()
    {
        Hand.Clear();
        Home.TraversalCurrentSlots().Do(s => s.Skill = null);
    }

    public List<SkillEntry> DrawSkills(SkillEntryCollectionDescriptor d)
    {
        List<SkillEntry> toRet = new();
        
        SkillPool.Shuffle();
        for (int i = 0; i < d.Count; i++)
        {
            SkillPool.TryPopItem(out SkillEntry item, s =>
            {
                if (!d.Pred(s))
                    return false;

                if (d.Distinct && toRet.Contains(s))
                    return false;

                return true;
            });

            item ??= Encyclopedia.SkillCategory[0];
            toRet.Add(item);
        }

        if (!d.Consume)
            SkillPool.Populate(toRet.FilterObj(s => s != Encyclopedia.SkillCategory[0]));

        return toRet;
    }
    
    public SkillEntry DrawSkill(SkillEntryDescriptor descriptor)
    {
        SkillPool.Shuffle();
        SkillPool.TryPopItem(out SkillEntry skillEntry, descriptor.Contains);
        skillEntry ??= Encyclopedia.SkillCategory[0];
        return skillEntry;
    }
    
    private RunSkill CreateSkill(SkillEntry skillEntry, JingJie? preferredJingJie = null)
    {
        JingJie jingJie = Mathf.Clamp(preferredJingJie ?? JingJie.LianQi, skillEntry.LowestJingJie, skillEntry.HighestJingJie);
        return RunSkill.FromEntryJingJie(skillEntry, jingJie);
    }
    
    private void AddSkill(RunSkill skill, DeckIndex? preferredDeckIndex = null)
    {
        if (!preferredDeckIndex.HasValue)
        {
            Hand.Add(skill);
            return;
        }

        DeckIndex deckIndex = preferredDeckIndex.Value;
        if (deckIndex.InField)
            Home.GetSlot(deckIndex.Index).Skill = skill;
        else
            Hand.Replace(deckIndex.Index, skill);
    }

    public void DrawSkillsProcedure(SkillEntryCollectionDescriptor descriptor)
    {
        List<SkillEntry> entries = DrawSkills(descriptor);
        entries.Do(e => AddSkill(CreateSkill(e, descriptor.JingJie)));
    }

    public void DrawSkillProcedure(SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null)
        => AddSkill(CreateSkill(DrawSkill(descriptor), descriptor.JingJie), preferredDeckIndex);

    public void AddSkillProcedure(SkillEntry skillEntry, JingJie? preferredJingJie = null,
        DeckIndex? preferredDeckIndex = null)
        => AddSkill(CreateSkill(skillEntry, preferredJingJie), preferredDeckIndex);

    #endregion
}
