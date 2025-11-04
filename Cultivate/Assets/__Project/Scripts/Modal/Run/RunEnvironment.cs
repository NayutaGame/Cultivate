
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

[Serializable]
public class RunEnvironment : Addressable, RunClosureListener, ISerializationCallbackReceiver
{
    #region Neurons

    private void InitNeurons()
    {
        StartRunNeuron = new();
        
        EquipNeuron = new();
        SwapNeuron = new();
        UnequipNeuron = new();
        MergeNeuron = new();
        SubmitFromHandNeuron = new();
        SubmitFromFieldNeuron = new();
        WithdrawToHandNeuron = new();
        WithdrawToFieldNeuron = new();
        RequirementSwapNeuron = new();

        DragBeginRunSkill = new();
        DragEndRunSkill = new();
        
        GainSkillNeuron = new();
        PickDiscoveredSkillNeuron = new();
        RemoveSkillNeuron = new();
        SkillSetJingJieNeuron = new();
        ReplaceSkillNeuron = new();
        BuySkillNeuron = new();
        ExchangeSkillNeuron = new();
        GachaNeuron = new();
        SelectOptionNeuron = new();
        JingJieChangedNeuron = new();
        LevelChangedNeuron = new();
        RoomChangedNeuron = new();
        PanelChangedNeuron = new();
        SkillMovedNeuron = new();
        ResimulateNeuron = new();
        GainMingYuanNeuron = new();
        LoseMingYuanNeuron = new();
        GainGoldNeuron = new();
        LoseGoldNeuron = new();
        GainHealthNeuron = new();
        LoseHealthNeuron = new();
        EngageEnemyNeuron = new();
        AppendReportNeuron = new();
        CommitBattleNeuron = new();
        GuideFinishNeuron = new();
        
        SkillMovedNeuron.Join(SkillMovedInvokeResimulate);

        CommendProcessedNeuron = new();

        NarrativeTextChangedNeuron = new();
        CharacterNameChangedNeuron = new();
    }

    public Neuron StartRunNeuron;
    
    public Neuron<EquipDetails> EquipNeuron;
    public Neuron<SwapDetails> SwapNeuron;
    public Neuron<UnequipDetails> UnequipNeuron;
    public Neuron<MergeDetails> MergeNeuron;
    public Neuron<SubmitFromHandDetails> SubmitFromHandNeuron;
    public Neuron<SubmitFromFieldDetails> SubmitFromFieldNeuron;
    public Neuron<WithdrawToHandDetails> WithdrawToHandNeuron;
    public Neuron<WithdrawToFieldDetails> WithdrawToFieldNeuron;
    public Neuron<RequirementSwapDetails> RequirementSwapNeuron;

    public Neuron<RunSkill> DragBeginRunSkill;
    public Neuron DragEndRunSkill;
    
    public Neuron<GainSkillBuilder> GainSkillNeuron;
    public Neuron<PickDiscoveredSkillDetails> PickDiscoveredSkillNeuron;
    public Neuron<RemoveSkillDetails> RemoveSkillNeuron;
    public Neuron<SkillSetJingJieDetails> SkillSetJingJieNeuron;
    public Neuron<ReplaceSkillDetails> ReplaceSkillNeuron;
    public Neuron<BuySkillDetails> BuySkillNeuron;
    public Neuron<ExchangeSkillDetails> ExchangeSkillNeuron;
    public Neuron<GachaDetails> GachaNeuron;
    public Neuron<SelectOptionDetails> SelectOptionNeuron;
    public Neuron<JingJieChangedDetails> JingJieChangedNeuron;
    public Neuron LevelChangedNeuron;
    public Neuron<RoomChangedDetails> RoomChangedNeuron;
    public Neuron<PanelChangedDetails> PanelChangedNeuron;
    public Neuron<SkillMovedDetails> SkillMovedNeuron;
    public Neuron ResimulateNeuron;
    public Neuron<int> GainMingYuanNeuron;
    public Neuron<int> LoseMingYuanNeuron;
    public Neuron<int> GainGoldNeuron;
    public Neuron<int> LoseGoldNeuron;
    public Neuron<int> GainHealthNeuron;
    public Neuron<int> LoseHealthNeuron;
    public Neuron<EngageEnemyDetails> EngageEnemyNeuron;
    public Neuron AppendReportNeuron;
    public Neuron<bool> CommitBattleNeuron;
    public Neuron<Guide> GuideFinishNeuron;

    public Neuron<Commend> CommendProcessedNeuron;

    public Neuron<string> NarrativeTextChangedNeuron;
    public Neuron<string, bool> CharacterNameChangedNeuron;
    
    public void SkillMovedInvokeResimulate(SkillMovedDetails d)
    {
        if (d.FromIndex.Region == SkillRegion.Field || d.ToIndex.Region == SkillRegion.Field)
            ResimulateNeuron.Invoke();
    }
    
    #endregion

    #region Core
    
    [NonSerialized] private Memory _memory;
    [NonSerialized] private RunClosureDict _closureDict;
    [NonSerialized] private Dirty<StageResult> _simulateResult;
    
    [NonSerialized] private RunEntity _away;
    [NonSerialized] private bool _awayIsDummy;

    [NonSerialized] private DateTime _startTime;
    [NonSerialized] private TimeSpan _loadedTime;
    [NonSerialized] private TimeSpan _runFinishedTime;
    [NonSerialized] private RunReport _runReport;

    [SerializeField] private Version _version;
    [SerializeField] private double _miliseconds;

    [SerializeReference] private SerializableDictionary _intMemory;
    [SerializeReference] private RunConfig _config;
    [SerializeField] private JingJie _jingJie;
    [SerializeReference] private Map _map;
    
    private MapNodeListModel _mapNodes;
    private Dictionary<CharacterEntry, NPC> _npcDict;
    
    [SerializeReference] private SkillPool _skillPool;
    [NonSerialized] private MutatorPool _mutatorPool;
    [SerializeReference] private SkillInventory _hand;
    [SerializeField] private BoundedInt _gold;
    [SerializeReference] private RunEntity _home;
    [SerializeReference] private EntityEntry _huaShenBossEntity;
    [SerializeField] private RunResult _result;

    [SerializeReference] private List<AchievementEntry> _newlyUnlockedAchievements;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Config",                     thisObject => ((RunEnvironment)thisObject)._config },
        { "Home",                       thisObject => ((RunEnvironment)thisObject)._home },
        { "Away",                       thisObject => ((RunEnvironment)thisObject)._away },
        { "Map",                        thisObject => ((RunEnvironment)thisObject)._map },
        { "MapNodes",                   thisObject => ((RunEnvironment)thisObject)._mapNodes },
        { "Hand",                       thisObject => ((RunEnvironment)thisObject)._hand },
        { "ActivePanel",                thisObject => ((RunEnvironment)thisObject).Cell },
        { "MingYuanDescription",        thisObject => ((RunEnvironment)thisObject).GetMingYuanDescription() },
        { "GoldDescription",            thisObject => ((RunEnvironment)thisObject).GetGoldDescription() },
        { "HealthDescription",          thisObject => ((RunEnvironment)thisObject).GetHealthDescription() },
        { "JingJieDescription",         thisObject => ((RunEnvironment)thisObject).GetJingJieDescription() },
        { "DifficultyDescription",      thisObject => ((RunEnvironment)thisObject).GetDifficultyDescription() },
    };
    public object Get(string s) => Accessor[s](this);
    private RunEnvironment(RunConfig config)
    {
        _version = AppManager.Version;
        _startTime = DateTime.Now;
        
        InitNeurons();

        _intMemory = new();
        _config = config;
        _jingJie = JingJie.LianQi;
        _map = new(_config.MapEntry);

        _mapNodes = new();
        _npcDict = new Dictionary<CharacterEntry, NPC>();
        Encyclopedia.CharacterCategory.Do(characterEntry =>
        {
            _npcDict.Add(characterEntry, new NPC(characterEntry));
        });
        
        _skillPool = new();
        _hand = new();
        _gold = new(0);
        _huaShenBossEntity = null;
        _result = new();
        _newlyUnlockedAchievements = new();

        InitializeCommonState();
    }

    private void InitializeCommonState()
    {
        _memory = new();
        _closureDict = new();
        _simulateResult = new(Simulate);
        
        SetHome(_home ?? RunEntity.Default());
        SetAway(null);
        
        ResimulateNeuron.Add(_simulateResult.SetDirty);
        SkillMovedNeuron.Add(GuideProcedure);

        if (AppManager.Instance.AudienceIsTester())
        {
            _runReport = new RunReport(this);
        
            RoomChangedNeuron.Add(AppendRoomReport);
            EngageEnemyNeuron.Add(AppendBattleReport);
            PanelChangedNeuron.Add(AppendPickDiscoveredSkillReport);
        }
        
        if (AllowMutate())
        {
            _mutatorPool = new();
        }
    }
    
    private bool AllowMutate()
        => _config.DifficultyProfile.GetEntry().AllowMutate;

    private void Deinit()
    {
        if (AppManager.Instance.AudienceIsTester())
        {
            TestReport lastReport = _runReport.GetCurrReport();
            lastReport?.OnExit(this);
        
            _runReport = null;
        
            RoomChangedNeuron.Remove(AppendRoomReport);
            EngageEnemyNeuron.Remove(AppendBattleReport);
            PanelChangedNeuron.Remove(AppendPickDiscoveredSkillReport);
        }
    }

    public static RunEnvironment FromConfig(RunConfig config)
        => new(config);

    #endregion

    #region Accessors

    public Memory Memory => _memory;
    public SerializableDictionary IntMemory => _intMemory;
    public RunConfig GetRunConfig() => _config;
    public Map Map => _map;
    public RunEntity Home => _home;
    public RunEntity Away => _away;
    public JingJie JingJie => _jingJie;
    public SkillPool SkillPool => _skillPool;
    public MutatorPool MutatorPool => _mutatorPool;
    public SkillInventory Hand => _hand;
    public void SendEvent(int eventId, RunClosureDetails closureDetails) => _closureDict.SendEvent(eventId, closureDetails);
    public StageResult GetSimulateResult() => _simulateResult.Value;
    public RunResult GetResult() => _result;
    public Sprite GetCurrEventIllustration() => _map.GetCurrEventIllustration();
    public TimeSpan GetRunfinishedTime() => _runFinishedTime;
    public TimeSpan GetPassedTime() => _loadedTime + (DateTime.Now - _startTime);
    public RunReport GetRunReport() => _runReport;
    public BoundedInt GetGold() => _gold;
    public MingYuan GetMingYuan() => _home.GetMingYuan();
    
    public bool IsPlayerInitiate() => !_config.DifficultyProfile.GetEntry().EnemyInitiate;

    public bool IsCompatible()
        => Version.IsRunCompatible(_version);

    public void SetHome(RunEntity home)
    {
        _home?.ChangedNeuron.Remove(ResimulateNeuron);
        _home = home;
        _home?.ChangedNeuron.Add(ResimulateNeuron);
    }

    public void SetAway(RunEntity away)
    {
        _awayIsDummy = away == null;
        away ??= RunEntity.FromJingJieHealth(_home.GetJingJie(), 1000000);
        
        _away?.ChangedNeuron.Remove(ResimulateNeuron);
        _away = away;
        _away?.ChangedNeuron.Add(ResimulateNeuron);

        EngageEnemyDetails d = new(_awayIsDummy, _away);
        EngageEnemyNeuron.Invoke(d);
    }

    public bool AwayIsDummy() => _awayIsDummy;

    public EntityEntry HuaShenBossEntity
    {
        get => _huaShenBossEntity;
        set => _huaShenBossEntity = value;
    }

    public void Register()
    {
        RegisterList(_config.GetCharacter()._runClosures);

        DifficultyEntry difficultyEntry = _config.DifficultyProfile.GetEntry();
        RegisterList(difficultyEntry._runClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            RegisterList(additionalDifficultyEntry._runClosures);

        AppManager.Instance.ProfileManager.GetCurrProfile().RegisterRunClosures(_closureDict);
    }

    private void RegisterList(RunClosure[] list)
    {
        list.Do(e => _closureDict.Register(this, e));
    }

    public void RegisterList(RunClosure[] list, RunClosureListener listener)
    {
        list.Do(e => _closureDict.Register(listener, e));
    }

    public void Unregister()
    {
        UnregisterList(_config.GetCharacter()._runClosures);

        DifficultyEntry difficultyEntry = _config.DifficultyProfile.GetEntry();
        UnregisterList(difficultyEntry._runClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            UnregisterList(additionalDifficultyEntry._runClosures);

        AppManager.Instance.ProfileManager.GetCurrProfile().UnregisterRunClosures(_closureDict);
    }

    private void UnregisterList(RunClosure[] list)
    {
        list.Do(e => _closureDict.Unregister(this, e));
    }

    public void UnregisterList(RunClosure[] list, RunClosureListener listener)
    {
        list.Do(e => _closureDict.Unregister(listener, e));
    }

    public RunSkill SkillFromDeckIndex(DeckIndex deckIndex)
    {
        switch (deckIndex.Region)
        {
            case SkillRegion.Hand:
                return Hand[deckIndex.Index];
            case SkillRegion.Field:
                return Home.GetSlot(deckIndex.Index).Skill;
            case SkillRegion.Requirement:
                if (Cell?.AsCell() is RequireCell requireCell)
                {
                    return requireCell.RequirementSlotList[deckIndex.Index].Skill;
                }
                else
                {
                    throw new NotImplementedException();
                }
        }

        return null;
    }

    public SkillSlot SkillSlotFromDeckIndex(DeckIndex deckIndex)
    {
        switch (deckIndex.Region)
        {
            case SkillRegion.Hand:
                return null;
            case SkillRegion.Field:
                return Home.GetSlot(deckIndex.Index);
            case SkillRegion.Requirement:
                return null;
        }

        return null;
    }

    public RequirementSlot RequirementSlotFromDeckIndex(DeckIndex deckIndex)
    {
        switch (deckIndex.Region)
        {
            case SkillRegion.Hand:
                return null;
            case SkillRegion.Field:
                return null;
            case SkillRegion.Requirement:
                if (Cell?.AsCell() is RequireCell requireCell)
                {
                    return requireCell.RequirementSlotList[deckIndex.Index];
                }
                else
                {
                    throw new NotImplementedException();
                }
        }

        return null;
    }

    public DeckIndex? DeckIndexFromSkill(RunSkill runSkill)
    {
        int? handIndex = Hand.FirstIdx(skill => skill == runSkill);
        if (handIndex.HasValue)
            return DeckIndex.FromHand(handIndex.Value);

        int? slotIndex = Home.TraversalCurrentSlots().FirstIdx(slot => slot.Skill == runSkill);
        if (slotIndex.HasValue)
            return DeckIndex.FromField(slotIndex.Value);
        
        if (Cell?.AsCell() is RequireCell cardPickerCell)
        {
            int? requirementIndex = cardPickerCell.RequirementSlotList.FirstIdx(slot => slot.Skill == runSkill);
            if (requirementIndex.HasValue)
                return DeckIndex.FromRequirement(requirementIndex.Value);
        }
        
        return null;
    }

    public bool DeckIndexFromQuery(out DeckIndex result, RunSkillQuery query, bool excludingField = false, bool excludingHand = false, DeckIndex[] omit = null)
    {
        omit ??= Array.Empty<DeckIndex>();
        
        result = default;
        
        foreach (DeckIndex deckIndex in TraversalDeckIndices(excludingField, excludingHand))
        {
            if (omit.Contains(deckIndex))
                continue;
            RunSkill skill = SkillFromDeckIndex(deckIndex);
            if (skill != null && query.Matches(skill))
            {
                result = deckIndex;
                return true;
            }
        }

        return false;
    }

    public bool DeckIndexFromQuery(out DeckIndex result, SkillEntryQuery query, bool excludingField = false, bool excludingHand = false, DeckIndex[] omit = null)
    {
        omit ??= Array.Empty<DeckIndex>();
        
        result = default;
        
        foreach (DeckIndex deckIndex in TraversalDeckIndices(excludingField, excludingHand))
        {
            if (omit.Contains(deckIndex))
                continue;
            RunSkill skill = SkillFromDeckIndex(deckIndex);
            if (skill != null && query.Matches(skill))
            {
                result = deckIndex;
                return true;
            }
        }

        return false;
    }
    
    public IEnumerable<DeckIndex> TraversalDeckIndices(bool excludingField = false, bool excludingHand = false)
    {
        if (!excludingField)
            foreach (var slot in RunManager.Instance.Environment.Home.TraversalCurrentSlots())
                yield return new DeckIndex(SkillRegion.Field, slot.GetIndex());
        if (!excludingHand)
            for (int i = 0; i < RunManager.Instance.Environment.Hand.Count(); i++)
                yield return new DeckIndex(SkillRegion.Hand, i);
    }

    public IEnumerable<RunSkill> TraversalSkills(bool excludingField = false, bool excludingHand = false)
    {
        if (!excludingField)
            foreach (var slot in RunManager.Instance.Environment.Home.TraversalCurrentSlots())
                if (slot.Skill != null)
                    yield return slot.Skill;
        if (!excludingHand)
            for (int i = 0; i < RunManager.Instance.Environment.Hand.Count(); i++)
                yield return _hand[i];
    }

    public void SetGuideToFinish()
    {
        // TODO
        // GetPanel().SetGuideToFinish();
    }

    private Hint GetMingYuanDescription()
        => new(GetMingYuan().GetMingYuanPenaltyText());

    private Hint GetGoldDescription()
        => new("金钱");

    private Hint GetHealthDescription()
        => new("气血上限\n战斗开始的气血");

    private Hint GetJingJieDescription()
        => new("有五个境界：练气，筑基，金丹，元婴，化神");

    private Hint GetDifficultyDescription()
        => new(_config.DifficultyProfile.GetEntry().InheritedDescription);

    public bool IsFinalJingJie()
        => _jingJie == _config.DifficultyProfile.GetEntry().FinalJingJie;

    public void RecordNewlyUnlockedAchievement(AchievementEntry achievementEntry)
    {
        _newlyUnlockedAchievements.Add(achievementEntry);
        AppManager.Instance.ProfileManager.GetCurrProfile().UnlockAchievement(achievementEntry);
    }

    public IEnumerable<AchievementProfile> TraversalNewlyUnlockedAchievements()
    {
        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        return _newlyUnlockedAchievements.Map(entry =>
        {
            LockIndex lockIndex = entry.GetLockIndex().Value;
            return profile.GetAchievementProfileFromLockIndex(lockIndex);
        });
    }
    
    public int GetIndexOfMapNode(MapNode mapNode)
        => _mapNodes.IndexOf(mapNode);

    #endregion

    #region Procedures

    public void StartRunProcedure(StartRunDetails d)
    {
        InitSkillPool();

        MapEntry mapEntry = Map.GetEntry();
        
        SetJingJieProcedure(mapEntry._envJingJie);
        _home.SetSlotCount(mapEntry._slotCount);
        SetDGoldProcedure(mapEntry._gold);

        DrawSkillsProcedure(
            SkillEntryQuery.FromBaseJingJieBound(
                    new(JingJie.LianQi, mapEntry._skillJingJie))
                .Stack(mapEntry._skillCount), mapEntry._skillJingJie);
        
        mapEntry.OnStartRun(this);

        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        Map.Init(profile, this);
        InitPanelFromCreation();
        
        SendEvent(RunClosureDict.START_RUN, d);
        StartRunNeuron.Invoke();

        AppManager.Instance.ProfileManager.GetCurrProfile().Environment = this;
    }

    public void ContinueRunProcedure(ContinueRunDetails d)
    {
        SetJingJieProcedure(_jingJie);
        
        InitPanelFromLoad();
    }

    private void InitSkillPool()
    {
        6.Do(_ => _config.PacksToStartWith.Do(pack =>
        {
            SkillPool.Populate(pack.Cards);
        }));
        
        SkillPool.Shuffle();
    }

    public void DepleteProcedure()
    {
        DepleteDetails d = new(_home);

        SendEvent(RunClosureDict.WIL_DEPLETE, d);

        if (d.Cancel)
            return;

        int preservedCount = d.PreserveFirstDeplete ? 1 : 0;
        foreach (SkillSlot slot in d.DepletingSlots)
        {
            RunSkill skill = slot.Skill;

            if (preservedCount > 0)
            {
                preservedCount--;
                continue;
            }

            d.DepletedSkills.Add(skill);
            slot.Skill = null;
        }

        SendEvent(RunClosureDict.DID_DEPLETE, d);
    }

    public void NextJingJieProcedure()
        => SetJingJieProcedure(JingJie + 1);
    
    public void SetJingJieProcedure(JingJie toJingJie)
        => SetJingJieProcedure(new JingJieChangedDetails(JingJie, toJingJie));
    
    private void SetJingJieProcedure(JingJieChangedDetails d)
    {
        SendEvent(RunClosureDict.WIL_JINGJIE_CHANGE, d);
        if (d.Cancel)
            return;

        _jingJie = d.ToJingJie;
        
        // move to ascension procedure
        SetHealthDetails setHealthDetails = SetHealthDetails.FromJingJieChange(d.FromJingJie, d.ToJingJie);
        SetHealthProcedure(setHealthDetails);
        
        _home.SetJingJie(d.ToJingJie);
        AudioManager.Play(d.ToJingJie.GetAudio());

        SendEvent(RunClosureDict.DID_JINGJIE_CHANGE, d);
        
        JingJieChangedNeuron.Invoke(d);
    }

    private StageResult Simulate()
    {
        PlacementProcedure();
        FormationProcedure();
        SecondPlacementProcedure();
        
        return StageResult.FromConfig(StageConfig.ForSimulate(_home, _away, _config));
    }

    private void PlacementProcedure()
    {
        if (RunManager.Instance.Environment.IsPlayerInitiate())
        {
            _home.PlacementProcedure();
            _away.PlacementProcedure();
        }
        else
        {
            _away.PlacementProcedure();
            _home.PlacementProcedure();
        }
    }

    private void FormationProcedure()
    {
        if (RunManager.Instance.Environment.IsPlayerInitiate())
        {
            PlayerInitiateFormationProcedure();
        }
        else
        {
            EnemyInitiateFormationProcedure();
        }
    }

    private void PlayerInitiateFormationProcedure()
    {
        bool homeAllowFormation = _config.DifficultyProfile.GetEntry().HomeAllowFormation;
        if (homeAllowFormation)
            _home.FormationProcedure();
        else
            _home.ClearFormationProcedure();

        bool awayAllowFormation = _config.DifficultyProfile.GetEntry().AwayAllowFormation;
        if (awayAllowFormation)
            _away.FormationProcedure();
        else
            _away.ClearFormationProcedure();
    }

    private void EnemyInitiateFormationProcedure()
    {
        bool awayAllowFormation = _config.DifficultyProfile.GetEntry().AwayAllowFormation;
        if (awayAllowFormation)
            _away.FormationProcedure();
        else
            _away.ClearFormationProcedure();
        
        bool homeAllowFormation = _config.DifficultyProfile.GetEntry().HomeAllowFormation;
        if (homeAllowFormation)
            _home.FormationProcedure();
        else
            _home.ClearFormationProcedure();
    }

    private void SecondPlacementProcedure()
    {
        if (RunManager.Instance.Environment.IsPlayerInitiate())
        {
            _home.SecondPlacementProcedure();
            _away.SecondPlacementProcedure();
        }
        else
        {
            _away.SecondPlacementProcedure();
            _home.SecondPlacementProcedure();
        }
    }

    public MergeTarget GetMergePreresult(RunSkill lhs, RunSkill rhs)
    {
        MergeDetails d = MergeDetails.ForDryRun(lhs, rhs, _home.GetJingJie());
        d.CalcMergeTarget();
        return d.MergeTarget;
    }
    
    public void SetPlayerEqualPreset(RunEntity template, bool toField, bool overwrite)
    {
        if (overwrite)
        {
            Home.SetSlotCount(template.GetSlotCount());
            SetHealthProcedure(template.GetHealth());
            ClearDeckProcedure();
        }

        GainSkillBuilder b = new();
        
        template.TraversalCurrentSlots().Do(s =>
        {
            SkillEntry entry = s.Skill?.GetEntry();
            if (entry == null)
                return;

            if (toField)
            {
                b.Pick(SkillGhost.FromEntryJingJie(entry, s.Skill.GetJingJie()), s.ToDeckIndex());
            }
            else
            {
                b.Pick(SkillGhost.FromEntryJingJie(entry, s.Skill.GetJingJie()));
            }
        });
        
        b.Execute();
        b.Invoke();
    }

    public void ClearDeckProcedure()
    {
        for (int i = _hand.Count() - 1; i >= 0; i--)
        {
            DeckIndex deckIndex = DeckIndex.FromHand(i);
            RemoveSkillProcedure(deckIndex);
        }

        foreach (SkillSlot slot in _home.TraversalCurrentSlots())
        {
            if (slot.Skill == null)
                return;
            RemoveSkillProcedure(slot.ToDeckIndex());
        }
    }

    public void CombatNormal()
    {
        SetGuideToFinish();
        AppManager.Instance.Push(AppStateMachine.STAGE, StageConfig.ForCombatNormal(_home, _away, _config));
    }

    public void CombatOnlyAnimation()
    {
        SetGuideToFinish();
        AppManager.Instance.Push(AppStateMachine.STAGE, StageConfig.ForCombatOnlyAnimation(_home, _away, _config));
    }

    public void CombatOnlyResult()
    {
        SetGuideToFinish();
        
        StageResult result = StageResult.FromConfig(StageConfig.ForCombatOnlyResult(_home, _away, _config));
        RunManager.Instance.Environment.ReceiveSignalProcedure(new SkipCombatSignal(result.Flag == 1));
    }
    
    public void SetDMingYuanProcedure(int value)
        => SetDMingYuanProcedure(new SetDMingYuanDetails(value));
    
    private void SetDMingYuanProcedure(SetDMingYuanDetails d)
    {
        if (d.Value == 0)
            return;
        
        SendEvent(RunClosureDict.WIL_SET_D_MINGYUAN, d);

        if (d.Cancel)
            return;

        GetMingYuan().Curr += d.Value;
        SendEvent(RunClosureDict.DID_SET_D_MINGYUAN, d);
        if (d.Value >= 0)
            GainMingYuanNeuron.Invoke(d.Value);
        else
            LoseMingYuanNeuron.Invoke(d.Value);

        // register this as a defeat check
        if (GetMingYuan().Curr <= 0)
            CommitRunProcedure(RunResult.RunOutcome.Defeated);
    }

    public void SetDGoldProcedure(int value)
        => SetDGoldProcedure(new SetDGoldDetails(value));
    
    private void SetDGoldProcedure(SetDGoldDetails d)
    {
        if (d.Value == 0)
            return;
        
        SendEvent(RunClosureDict.WIL_SET_D_GOLD, d);

        if (d.Cancel)
            return;

        _gold.Curr += d.Value;
        SendEvent(RunClosureDict.DID_SET_D_GOLD, d);
        if (d.Value >= 0)
            GainGoldNeuron.Invoke(d.Value);
        else
            LoseGoldNeuron.Invoke(d.Value);
    }
    
    public void GainHealthProcedure(int gain)
    {
        if (gain <= 0) return;
        SetHealthProcedure(SetHealthDetails.FromGain(gain));
    }

    public void LoseHealthProcedure(int lose)
    {
        if (lose <= 0) return;
        SetHealthProcedure(SetHealthDetails.FromLose(lose));
    }

    public void SetHealthProcedure(int value)
    {
        SetHealthProcedure(SetHealthDetails.FromDirect(value));
    }

    private void SetHealthProcedure(SetHealthDetails d)
    {
        if (d.Value == _home.GetHealth())
            return;
        
        SendEvent(RunClosureDict.WIL_SET_HEALTH, d);

        if (d.Cancel)
            return;

        _home.SetHealth(d.Value);
        
        SendEvent(RunClosureDict.DID_SET_HEALTH, d);
        if (d.Diff >= 0)
            GainHealthNeuron.Invoke(d.Diff);
        else
            LoseHealthNeuron.Invoke(d.Diff);
    }

    public void SetMaxMingYuanProcedure(int value)
        => SetMaxMingYuanProcedure(new SetMaxMingYuanDetails(value));
    
    private void SetMaxMingYuanProcedure(SetMaxMingYuanDetails d)
    {
        SendEvent(RunClosureDict.WIL_SET_MAX_MINGYUAN, d);

        if (d.Cancel)
            return;

        int diff = GetMingYuan().Curr - d.Value;
        if (diff > 0)
            SetDMingYuanProcedure(diff);

        GetMingYuan().UpperBound = d.Value;

        SendEvent(RunClosureDict.DID_SET_MAX_MINGYUAN, d);
    }

    public void DiscoverSkillProcedure(DiscoverSkillDetails d)
    {
        SendEvent(RunClosureDict.WIL_DISCOVER_SKILL, d);

        GainSkillBuilder b = new();
        b.Draw(d.DrawStrategies, d.PreferredJingJie, distinct: true, consume: true);
        if (AllowMutate())
            b.DrawMutator(JingJie);

        b.GainingSkills.Do(g => d.Skills.Add(SkillGhost.FromGainingSkill(g)));

        SendEvent(RunClosureDict.DID_DISCOVER_SKILL, d);
    }

    public void ExitShopProcedure()
    {
        ReceiveSignalProcedure(new ExitShopSignal());
    }

    public void SelectOptionProcedure(SelectOptionDetails d)
    {
        SelectOptionNeuron.Invoke(d);
        ReceiveSignalProcedure(new SelectedOptionSignal(d.SelectedIndex));
    }
    
    #endregion

    #region Narrative

    public void SetCharacterNameProcedure(string characterName, bool isHome)
    {
        NarrativeCell narrativeCell = Cell?.AsCell() as NarrativeCell;
        if (narrativeCell == null)
            return;
            
        narrativeCell.SetCharacterName(characterName, isHome);
        // 通知NarrativePanel更新
    }

    public void SetNarrativeTextProcedure(string narrativeText)
    {
        NarrativeCell narrativeCell = Cell?.AsCell() as NarrativeCell;
        if (narrativeCell == null)
            return;
            
        narrativeCell.SetNarrativeText(narrativeText);
        // 通知NarrativePanel更新
    }

    #endregion

    #region MoveSkillRelated

    public void MoveSkillProcedure(IDeckIndex fromIndex, IDeckIndex toIndex)
    {
        SkillMovedDetails d = new SkillMovedDetails(fromIndex.Reify(), toIndex.Reify());
        if (d.FromIndex == d.ToIndex)
            return;

        int branch = ((int)d.FromIndex.Region * 3) + (int)d.ToIndex.Region;
        Action<SkillMovedDetails>[] branches = {
            FromHandToHand,
            FromHandToField,
            FromHandToRequirement,
            FromFieldToHand,
            FromFieldToField,
            FromFieldToRequirement,
            FromRequirementToHand,
            FromRequirementToField,
            FromRequirementToRequirement,
        };

        branches[branch](d);
    }

    private void FromHandToHand(SkillMovedDetails d)
    {
        RunSkill fromSkill = SkillFromDeckIndex(d.FromIndex);
        RunSkill toSkill = SkillFromDeckIndex(d.ToIndex);
        MergeProcedure(MergeDetails.ForActualRun(fromSkill, toSkill, _home.GetJingJie()));
    }

    public void MergeProcedure(MergeDetails d)
    {
        // buggy behaviour, event is stateful
        SendEvent(RunClosureDict.WIL_MERGE, d);

        if (d.Cancel)
            return;
        
        d.CalcMergeTarget();
        Assert.IsTrue(d.State != MergeDetails.MergeState.Continue);

        if (d.State == MergeDetails.MergeState.Cancel)
            return;

        d.Cancel |= d.IsDryRun;
        if (d.Cancel)
            return;
        
        Assert.IsFalse(d.MergeTarget is InvalidMergeTarget);
        
        d.ExecuteSideEffects();
        d.MergeTarget.Execute(d, Hand);
        
        SendEvent(RunClosureDict.DID_MERGE, d);
        
        MergeNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    private void FromHandToField(SkillMovedDetails d)
    {
        RunSkill skill = SkillFromDeckIndex(d.FromIndex);
        SkillSlot slot = SkillSlotFromDeckIndex(d.ToIndex);
        EquipProcedure(new(skill, slot));
    }

    public void EquipProcedure(EquipDetails d)
    {
        RunSkill fromSkill = d.Skill;
        RunSkill toSkill = d.SkillSlot.Skill;

        if (toSkill == null)
        {
            Hand.Remove(fromSkill);
            d.SkillSlot.Skill = fromSkill;
        }
        else
        {
            Hand.Replace(fromSkill, toSkill);
            d.SkillSlot.Skill = fromSkill;
        }

        EquipNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    private void FromHandToRequirement(SkillMovedDetails d)
    {
        RunSkill skill = SkillFromDeckIndex(d.FromIndex);
        RequirementSlot slot = RequirementSlotFromDeckIndex(d.ToIndex);
        if (!slot.GetQuery().Matches(skill))
            return;
        
        SubmitFromHandProcedure(new(skill, slot));
    }

    public void SubmitFromHandProcedure(SubmitFromHandDetails d)
    {
        RunSkill fromSkill = d.Skill;
        RunSkill toSkill = d.RequirementSlot.Skill;

        if (toSkill == null)
        {
            Hand.Remove(fromSkill);
            d.RequirementSlot.Skill = fromSkill;
        }
        else
        {
            Hand.Replace(fromSkill, toSkill);
            d.RequirementSlot.Skill = fromSkill;
        }

        SubmitFromHandNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    private void FromFieldToHand(SkillMovedDetails d)
    {
        SkillSlot slot = SkillSlotFromDeckIndex(d.FromIndex);
        if (slot.Skill == null)
            return;
        
        UnequipProcedure(UnequipDetails.FromSlot(slot));
    }

    public void UnequipProcedure(UnequipDetails d)
    {
        RunSkill toUnequip = d.SkillSlot.Skill;
        if (toUnequip == null)
            return;

        DeckIndex toDeckIndex = new NextHandDeckIndexDefinition().Reify();
        
        Hand.Add(toUnequip);
        d.SkillSlot.Skill = null;
        
        UnequipNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.DeckIndex, toDeckIndex));
    }

    private void FromFieldToField(SkillMovedDetails d)
    {
        SkillSlot fromSlot = SkillSlotFromDeckIndex(d.FromIndex);
        SkillSlot toSlot = SkillSlotFromDeckIndex(d.ToIndex);
        if (fromSlot.Skill == null)
            return;
        
        SwapProcedure(new(fromSlot, toSlot));
    }

    public void SwapProcedure(SwapDetails d)
    {
        RunSkill temp = d.FromSlot.Skill;
        d.FromSlot.Skill = d.ToSlot.Skill;
        d.ToSlot.Skill = temp;

        SwapNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    private void FromFieldToRequirement(SkillMovedDetails d)
    {
        SkillSlot skillSlot = SkillSlotFromDeckIndex(d.FromIndex);
        RequirementSlot requirementSlot = RequirementSlotFromDeckIndex(d.ToIndex);
        if (skillSlot.Skill == null)
            return;
        
        if (!requirementSlot.GetQuery().Matches(skillSlot.Skill))
            return;
        
        SubmitFromFieldProcedure(new(skillSlot, requirementSlot));
    }

    public void SubmitFromFieldProcedure(SubmitFromFieldDetails d)
    {
        RunSkill temp = d.FromSlot.Skill;
        d.FromSlot.Skill = d.ToSlot.Skill;
        d.ToSlot.Skill = temp;
        
        SubmitFromFieldNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    private void FromRequirementToHand(SkillMovedDetails d)
    {
        RequirementSlot slot = RequirementSlotFromDeckIndex(d.FromIndex);
        if (slot.Skill == null)
            return;
        
        WithdrawToHandProcedure(WithdrawToHandDetails.FromSlot(slot));
    }

    public void WithdrawToHandProcedure(WithdrawToHandDetails d)
    {
        RunSkill toUnequip = d.RequirementSlot.Skill;
        if (toUnequip == null)
            return;

        DeckIndex toDeckIndex = new NextHandDeckIndexDefinition().Reify();
        
        Hand.Add(toUnequip);
        d.RequirementSlot.Skill = null;
        
        WithdrawToHandNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.DeckIndex, toDeckIndex));
    }

    private void FromRequirementToField(SkillMovedDetails d)
    {
        RequirementSlot requirementSlot = RequirementSlotFromDeckIndex(d.FromIndex);
        SkillSlot skillSlot = SkillSlotFromDeckIndex(d.ToIndex);
        if (requirementSlot.Skill == null)
            return;

        if (skillSlot.IsOccupied())
        {
            bool backwardQualified = requirementSlot.GetQuery().Matches(skillSlot.Skill);
            if (!backwardQualified)
            {
                UnequipProcedure(UnequipDetails.FromSlot(skillSlot));
            }
        }
        
        WithdrawToFieldProcedure(new(requirementSlot, skillSlot));
    }

    public void WithdrawToFieldProcedure(WithdrawToFieldDetails d)
    {
        RunSkill temp = d.FromSlot.Skill;
        d.FromSlot.Skill = d.ToSlot.Skill;
        d.ToSlot.Skill = temp;
        
        WithdrawToFieldNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    private void FromRequirementToRequirement(SkillMovedDetails d)
    {
        RequirementSlot fromSlot = RequirementSlotFromDeckIndex(d.FromIndex);
        RequirementSlot toSlot = RequirementSlotFromDeckIndex(d.ToIndex);
        if (fromSlot.Skill == null)
            return;

        bool forwardQualified = toSlot.GetQuery().Matches(fromSlot.Skill);
        if (!forwardQualified)
            return;

        if (toSlot.Skill != null)
        {
            bool backwardQualified = fromSlot.GetQuery().Matches(toSlot.Skill);
            if (!backwardQualified)
            {
                WithdrawToHandProcedure(WithdrawToHandDetails.FromSlot(toSlot));
            }
        }
        
        RequirementSwapProcedure(new(fromSlot, toSlot));
    }

    public void RequirementSwapProcedure(RequirementSwapDetails d)
    {
        RunSkill temp = d.FromSlot.Skill;
        d.FromSlot.Skill = d.ToSlot.Skill;
        d.ToSlot.Skill = temp;
        
        RequirementSwapNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    #endregion
    
    #region GainSkillRelated
    
    public void PickSkillProcedure(SkillEntry skillEntry, JingJie preferredJingJie = null, DeckIndex? preferredDeckIndex = null)
    {
        GainSkillBuilder b = new();
        b.Pick(SkillGhost.FromEntryJingJie(skillEntry, preferredJingJie), preferredDeckIndex);
        b.Execute();
        b.Invoke();
    }
    
    public void DrawSkillProcedure(SkillEntryQuery drawStrategy, JingJie jingJie, DeckIndex? preferredDeckIndex = null)
    {
        GainSkillBuilder b = new();
        b.Draw(drawStrategy, jingJie, preferredDeckIndex);
        b.Execute();
        b.Invoke();
    }

    public void DrawSkillsProcedure(List<SkillEntryQuery> drawStrategies, JingJie jingJie)
    {
        GainSkillBuilder b = new();
        b.Draw(drawStrategies, jingJie);
        b.Execute();
        b.Invoke();
    }
    
    public void PickDiscoveredSkillProcedure(PickDiscoveredSkillDetails d)
    {
        GainSkillBuilder b = new();
        b.Pick(d.Skill.Clone());
        b.Execute();
        
        PickDiscoveredSkillNeuron.Invoke(d);
        ReceiveSignalProcedure(new PickDiscoveredSkillSignal(d.PickedIndex));
    }

    public void BuySkillProcedure(BuySkillDetails d)
    {
        GainSkillBuilder b = new();
        b.Pick(d.Commodity.Skill.Clone());
        b.Execute();

        d.DeckIndex = b.GainingSkills[0].GetDeckIndex().Reify();
        BuySkillNeuron.Invoke(d);
    }

    public void ExchangeSkillProcedure(ExchangeSkillDetails d)
    {
        ExchangeSkillNeuron.Invoke(d);
    }

    public void GachaProcedure(GachaDetails d)
    {
        GainSkillBuilder b = new();
        b.Pick(d.Skill.Clone());
        b.Execute();
        
        d.DeckIndex = b.GainingSkills[0].GetDeckIndex().Reify();
        GachaNeuron.Invoke(d);
    }
    
    public void ConfirmSelectionsProcedure(List<SkillGhost> skillReferences)
    {
        ReceiveSignalProcedure(new ConfirmSkillsSignal(skillReferences));
    }

    public void ConfirmDeckSelectionsProcedure()
    {
        ReceiveSignalProcedure(new ConfirmDeckSignal());
    }

    public void RemoveSkillProcedure(RunSkillQuery query)
    {
        bool isFound = DeckIndexFromQuery(out DeckIndex deckIndex, query);
        if (isFound)
            RemoveSkillProcedure(deckIndex);
    }

    public void RemoveSkillProcedure(RunSkill skill)
    {
        RemoveSkillProcedure(skill.ToDeckIndex());
    }

    public void RemoveSkillProcedure(DeckIndex deckIndex)
    {
        RemoveSkillDetails d = new(deckIndex);
        
        if (deckIndex.Region == SkillRegion.Field)
            Home.GetSlot(deckIndex.Index).Skill = null;
        else if (deckIndex.Region == SkillRegion.Hand)
            Hand.RemoveAt(deckIndex.Index);
        else if (deckIndex.Region == SkillRegion.Requirement)
        {
            RequireCell requireCell = Cell?.AsCell() as RequireCell;
            Assert.IsTrue(requireCell != null);
            requireCell.RequirementSlotList[deckIndex.Index].Skill = null;
        }

        RemoveSkillNeuron.Invoke(d);
    }

    public void SkillSetJingJieProcedure(JingJie jingJie, DeckIndex deckIndex)
    {
        SkillSetJingJieDetails d = new(jingJie, deckIndex);
        RunSkill template = SkillFromDeckIndex(deckIndex);
        ReplaceSkillProcedure(RunSkill.FromChangeJingJie(template, jingJie), deckIndex);
        SkillSetJingJieNeuron.Invoke(d);
    }

    public void ReplaceSkillProcedure(RunSkill template, DeckIndex deckIndex)
    {
        ReplaceSkillDetails d = new(template, deckIndex);
        
        if (deckIndex.Region == SkillRegion.Field)
            Home.GetSlot(deckIndex.Index).Skill = template.Clone();
        else if (deckIndex.Region == SkillRegion.Hand)
            Hand.Replace(deckIndex.Index, template.Clone());
        else if (deckIndex.Region == SkillRegion.Requirement)
        {
            RequireCell requireCell = Cell?.AsCell() as RequireCell;
            Assert.IsTrue(requireCell != null);
            requireCell.RequirementSlotList[deckIndex.Index].Skill = template.Clone();
        }

        ReplaceSkillNeuron.Invoke(d);
    }

    #endregion

    #region PanelOperations
    
    private RoomEnvironment _roomEnvironment;
    [NonSerialized] private ICellAdapter _cell;

    public enum RunState
    {
        MapSelecting,
        InRoom,
        Committed,
        Uncommon,
    }

    private RunState _runState = RunState.MapSelecting;

    public ICellAdapter Cell
    {
        get => _cell;
        private set
        {
            if (_cell == value)
                return;
            PanelChangedDetails panelChangedDetails = new(_cell, value);
        
            if (_cell != null)
                SendEvent(RunClosureDict.WIL_CHANGE_CELL, panelChangedDetails);
            _cell?.Exit();
            _cell = value;
            _cell?.Enter();
            if (_cell != null)
                PanelChangedNeuron.Invoke(panelChangedDetails);
        }
    }
    
    public void ReceiveSignalProcedure(Signal signal)
    {
        if (_runState == RunState.Committed)
            return;

        if (_runState == RunState.MapSelecting && signal is SelectedMapNodeSignal selectedMapNodeSignal)
        {
            _runState = RunState.InRoom;
            _roomEnvironment = RoomEnvironment.CreateRoom(selectedMapNodeSignal.MapNode, _jingJie, 0, _home);
            _roomEnvironment.Step();
            Cell = _roomEnvironment.CurrentCell;
            return;
        }

        if (_runState == RunState.InRoom)
        {
            _roomEnvironment.ReceiveSignal(signal);
            
            if (_roomEnvironment.IsFinished())
            {
                _runState = RunState.MapSelecting;
                _roomEnvironment = null;
                Cell = null;
            }
            else
            {
                Cell = _roomEnvironment.CurrentCell;
            }
            
            return;
        }
        
        if (_runState == RunState.Committed)
            return;
        
        if (_roomEnvironment.IsFinished())
        {
            if (Map.IsAboutToFinish())
            {
                CommitRunProcedure(RunResult.RunOutcome.Victorious);
                return;
            }
            else
            {
                Room oldRoom = Map.GetCurrRoom();
                bool levelChanged = Step();
                
                bool cond1 = Map.GetCurrRoom().GetDescriptor() is SuccessRoomDefinition;
                bool cond2 = Map.GetCurrRoom().GetDescriptor() is AscensionRoomDefinition && IsFinalJingJie();
                if (cond1 || cond2)
                {
                    CommitRunProcedure(RunResult.RunOutcome.Victorious);
                    return;
                }

                AppManager.Instance.ProfileManager.GetCurrProfile().Environment = this;
                
                ICellAdapter cell = Map.CreateCellFromCurrRoom();
                
                if (levelChanged)
                    LevelChangedNeuron.Invoke();

                Room newRoom = Map.GetCurrRoom();
                RoomChangedNeuron.Invoke(new(oldRoom, newRoom));

                Cell = cell;
                return;
            }
        }
    }

    public void GuideProcedure(SkillMovedDetails d)
        => GuideProcedure(new DeckChangedSignal(d.FromIndex.Reify(), d.ToIndex.Reify()));

    public void GuideProcedure(Signal signal)
    {
        // TODO
        // Guide guide = _cell.GetGuideDescriptor();
        // guide?.ReceiveSignal(_cell, signal);
        // if (guide != null)
        //     CanvasManager.Instance.RefreshGuide();
    }

    private bool Step()
    {
        if (Map.IsLastStep())
        {
            Map.NextLevel();
            return true;
        }

        Map.NextStep();
        return false;
    }

    public void CommitRunProcedure(RunResult.RunOutcome state)
    {
        if (_runState == RunState.Committed)
            return;

        _runState = RunState.Committed;
        _result.SetOutcome(state);
        _runFinishedTime = GetPassedTime();

        SendEvent(RunClosureDict.DID_COMMIT_RUN, new RunCommitDetails(this));
        
        CommitCell resultPanel = new CommitCell(this);
        Cell = resultPanel;
    }

    private void InitPanelFromCreation()
    {
    }

    private void InitPanelFromLoad()
    {
        Cell = Map.CreateCellFromCurrRoom();
        Room newRoom = Map.GetCurrRoom();
        RoomChangedNeuron.Invoke(new(null, newRoom));
    }

    #endregion

    #region Serialization

    public void PrintTime(string title = "Time")
    {
        Debug.Log(title);
        Debug.Log($"Passed Time = {Util.FormatTime(DateTime.Now - _startTime)}");
        Debug.Log($"Loaded Time = {Util.FormatTime(_loadedTime)}");
        Debug.Log($"Run Finished Time = {Util.FormatTime(_runFinishedTime)}");
    }

    public void WriteTime()
    {
        _loadedTime += DateTime.Now - _startTime;
        _startTime = DateTime.Now;
    }

    public void OnBeforeSerialize()
    {
        _miliseconds = _loadedTime.TotalMilliseconds;
    }

    public void OnAfterDeserialize()
    {
        _startTime = DateTime.Now;
        _loadedTime = TimeSpan.FromMilliseconds(_miliseconds);
        _jingJie = string.IsNullOrEmpty(_jingJie.GetId()) ? null : Encyclopedia.JingJieCategory.FromId(_jingJie.GetId());
        
        if (_huaShenBossEntity != null)
            _huaShenBossEntity = string.IsNullOrEmpty(_huaShenBossEntity.GetId()) ? null : Encyclopedia.EntityCategory.FromId(_huaShenBossEntity.GetId());
        
        InitNeurons();

        for (int i = 0; i < _newlyUnlockedAchievements.Count; i++)
        {
            AchievementEntry entry = _newlyUnlockedAchievements[i];
            entry = string.IsNullOrEmpty(entry.GetId()) ? null : Encyclopedia.AchievementCategory.FromId(entry.GetId());
            _newlyUnlockedAchievements[i] = entry;
        }

        InitializeCommonState();
    }

    #endregion

    #region ForTestAndReport

    public void PrintJson()
    {
        string json = JsonUtility.ToJson(this, true);
        Debug.Log(json);
        GUIUtility.systemCopyBuffer = json;
    }

    public void PrintDeck()
    {
        StringBuilder b = new();
        b.Append($"准备区：\n");
        foreach (SkillSlot slot in Home.TraversalCurrentSlots())
        {
            if (slot.Skill == null)
                b.Append("【空白】\t");
            else
                b.Append($"【{slot.Skill.GetEntry().GetName()}】\t");
        }

        b.Append("\n手牌区：\n");
        foreach (RunSkill skill in Hand)
        {
            b.Append($"【{skill.GetEntry().GetName()}】");
        }
        Debug.Log(b.ToString());
    }

    public void WriteIntoEditable()
    {
        EditorManager.Instance.Add(_home);
        int ladder = Map.GetCurrRoom().Ladder;
        _home.SetLadder(ladder);
        EditorManager.Instance.Save();
    }
    
    private void AppendRoomReport(RoomChangedDetails d)
    {
        TestReport oldReport = _runReport.GetCurrReport();
        oldReport?.OnExit(this);
        
        RoomReport newReport = RoomReport.FromEnvironment(this, d.ToRoom);
        
        _runReport.AppendReport(newReport);
        newReport.OnEnter(this);
        
        // _runReport.CopyRunReportToClipboard();

        AppendReportNeuron.Invoke();
    }
    
    private void AppendBattleReport(EngageEnemyDetails d)
    {
        if (d.IsDummy)
            return;
        
        TestReport oldReport = _runReport.GetCurrReport();
        oldReport?.OnExit(this);
        
        BattleReport newReport = BattleReport.FromEnvironment(this, _away);
        
        _runReport.AppendReport(newReport);
        newReport.OnEnter(this);
        
        // _runReport.CopyRunReportToClipboard();

        AppendReportNeuron.Invoke();
    }

    private void AppendPickDiscoveredSkillReport(PanelChangedDetails d)
    {
        DiscoverCell cell = d.ToPanel as DiscoverCell;
        if (cell == null)
            return;
        
        TestReport oldReport = _runReport.GetCurrReport();
        oldReport?.OnExit(this);
        
        PickDiscoveredSkillReport newReport = PickDiscoveredSkillReport.FromEnvironment(this);
        
        _runReport.AppendReport(newReport);
        newReport.OnEnter(this);
        
        // _runReport.CopyRunReportToClipboard();
    
        AppendReportNeuron.Invoke();
    }

    #endregion
}
