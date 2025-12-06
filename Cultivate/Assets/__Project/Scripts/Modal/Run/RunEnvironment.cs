
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLLibrary;
using UnityEngine;
using UnityEngine.Assertions;

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
        FromHandToBarterNeuron = new();
        FromFieldToBarterNeuron = new();
        FromBarterToHandNeuron = new();
        FromBarterToFieldNeuron = new();

        FromBoardToRightBucketNeuron = new();
        FromRightBucketToBoardNeuron = new();
        BarterClearRightBucketItemsNeuron = new();
        BarterWeightIsUpdatedNeuron = new();

        DragBeginRunSkill = new();
        DragEndRunSkill = new();
        
        GainSkillNeuron = new();
        PickDiscoveredSkillNeuron = new();
        RemoveSkillNeuron = new();
        SkillSetJingJieNeuron = new();
        SetSkillNeuron = new();
        BuySkillNeuron = new();
        ExchangeSkillNeuron = new();
        GachaNeuron = new();
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
    public Neuron<FromHandToBarterDetails> FromHandToBarterNeuron;
    public Neuron<FromFieldToBarterDetails> FromFieldToBarterNeuron;
    public Neuron<FromBarterToHandDetails> FromBarterToHandNeuron;
    public Neuron<FromBarterToFieldDetails> FromBarterToFieldNeuron;
    
    public Neuron<FromBoardToRightBucketDetails> FromBoardToRightBucketNeuron;
    public Neuron<FromRightBucketToBoardDetails> FromRightBucketToBoardNeuron;
    public Neuron BarterClearRightBucketItemsNeuron;
    public Neuron<int> BarterWeightIsUpdatedNeuron;

    public Neuron<RunSkill> DragBeginRunSkill;
    public Neuron DragEndRunSkill;
    
    public Neuron<GainSkillBuilder> GainSkillNeuron;
    public Neuron<PickDiscoveredSkillDetails> PickDiscoveredSkillNeuron;
    public Neuron<RemoveSkillDetails> RemoveSkillNeuron;
    public Neuron<SkillSetJingJieDetails> SkillSetJingJieNeuron;
    public Neuron<SetSkillDetails> SetSkillNeuron;
    public Neuron<BuySkillDetails> BuySkillNeuron;
    public Neuron<ExchangeSkillDetails> ExchangeSkillNeuron;
    public Neuron<GachaDetails> GachaNeuron;
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
    
    private RunCharacter _character;
    private Dictionary<CharacterEntry, RunNPC> _npcDict;
    
    [SerializeReference] private RunSkillListModel _hand;
    [SerializeField] private BoundedInt _gold;
    [SerializeReference] private EntityEntry _huaShenBossEntity;
    [SerializeField] private RunResult _result;
    
    [SerializeReference] private SkillPool _skillPool;
    [NonSerialized] private MutatorPool _mutatorPool;
    [SerializeReference] public EntityPool EntityPool;
    [SerializeReference] public RoomPool RoomPool;

    [SerializeReference] private List<AchievementEntry> _newlyUnlockedAchievements;
    // [Obsolete] [SerializeReference] private RunEntity _home;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Config",                     thisObject => ((RunEnvironment)thisObject)._config },
        { "Home",                       thisObject => ((RunEnvironment)thisObject)._character.Build },
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
        _npcDict = new Dictionary<CharacterEntry, RunNPC>();
        Encyclopedia.CharacterCategory.Do(characterEntry =>
        {
            _npcDict.Add(characterEntry, new RunNPC(characterEntry));
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

        _character ??= new(_config.GetCharacter());
        SetHome(Home ?? RunEntity.Default());
        Home.SetModel(_character.CharacterEntry.EntityEntry);
        
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
    public RunCharacter Character => _character;
    public RunEntity Home => _character.Build;
    public RunEntity Away => _away;
    public JingJie JingJie => _jingJie;
    public SkillPool SkillPool => _skillPool;
    public MutatorPool MutatorPool => _mutatorPool;
    public RunSkillListModel Hand => _hand;
    public void SendEvent(int eventId, RunClosureDetails closureDetails) => _closureDict.SendEvent(eventId, closureDetails);
    public StageResult GetSimulateResult() => _simulateResult.Value;
    public RunResult GetResult() => _result;
    public Sprite GetCurrEventIllustration() => _map.GetCurrEventIllustration();
    public TimeSpan GetRunfinishedTime() => _runFinishedTime;
    public TimeSpan GetPassedTime() => _loadedTime + (DateTime.Now - _startTime);
    public RunReport GetRunReport() => _runReport;
    public BoundedInt GetGold() => _gold;
    public int GetCurrGold() => _gold.Curr;
    public MingYuan GetMingYuan() => Home.GetMingYuan();
    public int GetCurrMingYuan() => Home.GetMingYuan().Curr;
    public int GetMaxMingYuan() => Home.GetMingYuan().UpperBound;
    
    public bool IsPlayerInitiate() => !_config.DifficultyProfile.GetEntry().EnemyInitiate;

    public bool IsCompatible()
        => Version.IsRunCompatible(_version);
    
    private bool AllowMutate()
        => _config.DifficultyProfile.GetEntry().AllowMutate;

    public void SetHome(RunEntity home)
    {
        _character.Build?.ChangedNeuron.Remove(ResimulateNeuron);
        _character.Build = home;
        _character.Build?.ChangedNeuron.Add(ResimulateNeuron);
    }

    public void SetAway(RunEntity away)
    {
        _awayIsDummy = away == null;
        away ??= RunEntity.FromJingJieHealth(Home.GetJingJie(), 1000000);
        
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
        RegisterList(_config.GetCharacter().RunClosures);

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
        UnregisterList(_config.GetCharacter().RunClosures);

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
            case SkillRegion.Barter:
                if (Cell?.AsCell() is BarterCell barterCell)
                {
                    return barterCell.LeftBucketItems[deckIndex.Index];
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
            case SkillRegion.Barter:
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
            case SkillRegion.Barter:
                return null;
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

        Cell cell = Cell?.AsCell();
        if (cell is RequireCell cardPickerCell)
        {
            int? requirementIndex = cardPickerCell.RequirementSlotList.FirstIdx(slot => slot.Skill == runSkill);
            if (requirementIndex.HasValue)
                return DeckIndex.FromRequirement(requirementIndex.Value);
        }

        if (cell is BarterCell barterCell)
        {
            int? barterIndex = barterCell.LeftBucketItems.FirstIdx(skill => skill == runSkill);
            if (barterIndex.HasValue)
                return DeckIndex.FromBarter(barterIndex.Value);
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

    private int _availableStepCount;
    private int _totalStepCount;

    public void StartRunProcedure(StartRunDetails d)
    {
        InitSkillPool();
        InitEntityPool();
        InitRoomPool();

        MapEntry mapEntry = Map.GetEntry();
        SetJingJieProcedure(mapEntry._envJingJie);
        Home.SetHealth(RunEntity.HealthFromJingJie[mapEntry._envJingJie]);
        Home.SetSlotCount(mapEntry._slotCount);
        GainGoldProcedure(mapEntry._gold);

        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        
        _runState = RunState.MapSelecting;
        Map.Init(profile, this);

        AdjustStepCountFromJingJie(mapEntry._envJingJie);
        
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

    public void AdjustStepCountFromJingJie(JingJie jingJie)
    {
        _availableStepCount = 3;
        _totalStepCount = 5;
    }

    private void InitSkillPool()
    {
        6.Do(_ => _config.PacksToStartWith.Do(pack =>
        {
            SkillPool.Populate(pack.Cards);
        }));
        
        SkillPool.Shuffle();
    }

    private void InitEntityPool()
    {
        EntityPool = new();
        int difficulty = GetRunConfig().GetDifficulty();
        EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.FilterObj(
            e => e.IsInPool() && e.GetAllowedDifficulty().Contains(difficulty)));
        EntityPool.Shuffle();
    }

    private void InitRoomPool()
    {
        RoomPool = new();
        int difficulty = GetRunConfig().GetDifficulty();
        RoomPool.Populate(Encyclopedia.LegacyRoomCategory.FilterObj(e => e.WithInPool && e.DifficultyBound.Contains(difficulty)));
        RoomPool.Shuffle();
    }

    public void DepleteProcedure()
    {
        DepleteDetails d = new(Home);

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
    
    public void SetJingJieProcedure(JingJie toJingJie)
        => SetJingJieProcedure(new JingJieChangedDetails(JingJie, toJingJie));
    
    public void SetJingJieProcedure(JingJieChangedDetails d)
    {
        SendEvent(RunClosureDict.WIL_JINGJIE_CHANGE, d);
        if (d.Cancel)
            return;

        _jingJie = d.ToJingJie;
        
        Home.SetJingJie(d.ToJingJie);

        SendEvent(RunClosureDict.DID_JINGJIE_CHANGE, d);
        JingJieChangedNeuron.Invoke(d);
        
        AudioManager.Play(d.ToJingJie.GetAudio());
    }

    private StageResult Simulate()
    {
        PlacementProcedure();
        FormationProcedure();
        SecondPlacementProcedure();
        
        return StageResult.FromConfig(StageConfig.ForSimulate(Home, _away, _config));
    }

    private void PlacementProcedure()
    {
        if (RunManager.Instance.Environment.IsPlayerInitiate())
        {
            Home.PlacementProcedure();
            _away.PlacementProcedure();
        }
        else
        {
            _away.PlacementProcedure();
            Home.PlacementProcedure();
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
            Home.FormationProcedure();
        else
            Home.ClearFormationProcedure();

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
            Home.FormationProcedure();
        else
            Home.ClearFormationProcedure();
    }

    private void SecondPlacementProcedure()
    {
        if (RunManager.Instance.Environment.IsPlayerInitiate())
        {
            Home.SecondPlacementProcedure();
            _away.SecondPlacementProcedure();
        }
        else
        {
            _away.SecondPlacementProcedure();
            Home.SecondPlacementProcedure();
        }
    }

    public MergeTarget GetMergePreresult(RunSkill lhs, RunSkill rhs)
    {
        MergeDetails d = MergeDetails.ForDryRun(lhs, rhs, Home.GetJingJie());
        d.CalcMergeTarget();
        return d.MergeTarget;
    }

    public void SetPlayerModelProcedure(string modelName)
    {
        EntityEntry model = Encyclopedia.EntityCategory.FromName(modelName);
        _character.Build.SetModel(model);
    }

    public void SetPlayerEqualPresetProcedure(string templateName, bool toField, bool overwrite)
        => SetPlayerEqualPresetProcedure(EditorManager.FindEntity(templateName), toField, overwrite);
    
    public void SetPlayerEqualPresetProcedure(RunEntity template, bool toField, bool overwrite)
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

        foreach (SkillSlot slot in Home.TraversalCurrentSlots())
        {
            if (slot.Skill == null)
                return;
            RemoveSkillProcedure(slot.ToDeckIndex());
        }
    }

    public void CombatNormal()
    {
        SetGuideToFinish();
        AppManager.Instance.Push(AppStateMachine.STAGE, StageConfig.ForCombatNormal(Home, _away, _config));
    }

    public void CombatOnlyAnimation()
    {
        SetGuideToFinish();
        AppManager.Instance.Push(AppStateMachine.STAGE, StageConfig.ForCombatOnlyAnimation(Home, _away, _config));
    }

    public void CombatOnlyResult()
    {
        SetGuideToFinish();
        
        StageResult result = StageResult.FromConfig(StageConfig.ForCombatOnlyResult(Home, _away, _config));
        RunManager.Instance.Environment.ReceiveSignalProcedure(new SkipCombatSignal(result.Flag == 1));
    }

    public void GainMingYuanProcedure(int value)
    {
        if (value <= 0) return;
        SetDMingYuanProcedure(value);
    }

    public void LoseMingYuanProcedure(int value)
    {
        if (value <= 0) return;
        SetDMingYuanProcedure(-value);
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

    public void GainGoldProcedure(int value)
    {
        if (value <= 0) return;
        SetDGoldProcedure(value);
    }

    public void LoseGoldProcedure(int value)
    {
        if (value <= 0) return;
        SetDGoldProcedure(-value);
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

    public void SetHealthProcedure(SetHealthDetails d)
    {
        if (d.Value == Home.GetHealth())
            return;
        
        SendEvent(RunClosureDict.WIL_SET_HEALTH, d);

        if (d.Cancel)
            return;

        Home.SetHealth(d.Value);
        
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

        int branch = ((int)d.FromIndex.Region * 4) + (int)d.ToIndex.Region;
        Action<SkillMovedDetails>[] branches = {
            FromHandToHand,
            FromHandToField,
            FromHandToRequirement,
            FromHandToBarter,
            FromFieldToHand,
            FromFieldToField,
            FromFieldToRequirement,
            FromFieldToBarter,
            FromRequirementToHand,
            FromRequirementToField,
            FromRequirementToRequirement,
            FromRequirementToBarter,
            FromBarterToHand,
            FromBarterToField,
            FromBarterToRequirement,
            FromBarterToBarter,
        };

        branches[branch](d);
    }

    private void FromHandToHand(SkillMovedDetails d)
    {
        RunSkill fromSkill = SkillFromDeckIndex(d.FromIndex);
        RunSkill toSkill = SkillFromDeckIndex(d.ToIndex);
        MergeProcedure(MergeDetails.ForActualRun(fromSkill, toSkill, Home.GetJingJie()));
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

    private void FromHandToBarter(SkillMovedDetails d)
    {
        FromHandToBarterProcedure(FromHandToBarterDetails.FromHandIndex(d.FromIndex));
    }

    public void FromHandToBarterProcedure(FromHandToBarterDetails d)
    {
        if (d.FromSkill == null)
            return;

        BarterCell barterCell = Cell?.AsCell() as BarterCell;
        Assert.IsTrue(barterCell != null);

        Hand.Remove(d.FromSkill);

        DeckIndex toDeckIndex = new NextBarterDeckIndexDefinition().Reify();
        barterCell.LeftBucketItems.Add(d.FromSkill);

        FromHandToBarterNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromIndex, toDeckIndex));
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
        if (fromSlot.Skill == null)
            return;
        
        SkillSlot toSlot = SkillSlotFromDeckIndex(d.ToIndex);
        
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
        if (skillSlot.Skill == null)
            return;
        
        RequirementSlot requirementSlot = RequirementSlotFromDeckIndex(d.ToIndex);
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

    private void FromFieldToBarter(SkillMovedDetails d)
    {
        SkillSlot skillSlot = SkillSlotFromDeckIndex(d.FromIndex);
        if (skillSlot.Skill == null)
            return;
        
        FromFieldToBarterProcedure(new FromFieldToBarterDetails(skillSlot, d.FromIndex));
    }

    public void FromFieldToBarterProcedure(FromFieldToBarterDetails d)
    {
        if (d.FromSlot == null || d.FromSlot.Skill == null)
            return;

        BarterCell barterCell = Cell?.AsCell() as BarterCell;
        Assert.IsTrue(barterCell != null);

        RunSkill skill = d.FromSlot.Skill;
        d.FromSlot.Skill = null;

        DeckIndex toDeckIndex = new NextBarterDeckIndexDefinition().Reify();
        barterCell.LeftBucketItems.Add(skill);

        FromFieldToBarterNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromIndex, toDeckIndex));
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
        if (requirementSlot.Skill == null)
            return;
        
        SkillSlot skillSlot = SkillSlotFromDeckIndex(d.ToIndex);

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
    
    private void FromRequirementToBarter(SkillMovedDetails d)
    {
        throw new NotImplementedException("Unexpected pathway");
    }
    
    private void FromBarterToHand(SkillMovedDetails d)
    {
        RunSkill fromSkill = SkillFromDeckIndex(d.FromIndex);
        FromBarterToHandProcedure(new FromBarterToHandDetails(fromSkill, d.FromIndex));
    }

    public void FromBarterToHandProcedure(FromBarterToHandDetails d)
    {
        if (d.FromSkill == null)
            return;

        BarterCell barterCell = Cell?.AsCell() as BarterCell;
        Assert.IsTrue(barterCell != null);

        DeckIndex toDeckIndex = new NextHandDeckIndexDefinition().Reify();

        barterCell.LeftBucketItems.RemoveAt(d.FromIndex.Index);
        Hand.Add(d.FromSkill);

        FromBarterToHandNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromIndex, toDeckIndex));
    }
    
    private void FromBarterToField(SkillMovedDetails d)
    {
        RunSkill fromSkill = SkillFromDeckIndex(d.FromIndex);
        SkillSlot toSlot = SkillSlotFromDeckIndex(d.ToIndex);

        if (toSlot.IsOccupied())
        {
            UnequipProcedure(UnequipDetails.FromSlot(toSlot));
        }
        
        FromBarterToFieldProcedure(new FromBarterToFieldDetails(fromSkill, d.FromIndex, toSlot, d.ToIndex));
    }

    public void FromBarterToFieldProcedure(FromBarterToFieldDetails d)
    {
        if (d.FromSkill == null || d.ToSlot == null)
            return;

        BarterCell barterCell = Cell?.AsCell() as BarterCell;
        Assert.IsTrue(barterCell != null);

        barterCell.LeftBucketItems.RemoveAt(d.FromIndex.Index);
        d.ToSlot.Skill = d.FromSkill;

        FromBarterToFieldNeuron.Invoke(d);
        SkillMovedNeuron.Invoke(new(d.FromIndex, d.ToIndex));
    }
    
    private void FromBarterToRequirement(SkillMovedDetails d)
    {
        throw new NotImplementedException("Unexpected pathway");
    }

    private void FromBarterToBarter(SkillMovedDetails d)
    {
        return;
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

    public void PickSkillsProcedure(List<SkillGhost> skillGhosts)
    {
        GainSkillBuilder b = new();
        foreach (SkillGhost skillGhost in skillGhosts)
            b.Pick(skillGhost);
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
        
        switch (deckIndex.Region)
        {
            case SkillRegion.Field:
                Home.GetSlot(deckIndex.Index).Skill = null;
                break;
            case SkillRegion.Hand:
                Hand.RemoveAt(deckIndex.Index);
                break;
            case SkillRegion.Requirement:
            {
                RequireCell requireCell = Cell?.AsCell() as RequireCell;
                Assert.IsTrue(requireCell != null);
                requireCell.RequirementSlotList[deckIndex.Index].Skill = null;
                break;
            }
            case SkillRegion.Barter:
            {
                BarterCell barterCell = Cell?.AsCell() as BarterCell;
                Assert.IsTrue(barterCell != null);
                barterCell.LeftBucketItems.RemoveAt(deckIndex.Index);
                break;
            }
        }

        RemoveSkillNeuron.Invoke(d);
    }

    public void SkillSetJingJieProcedure(JingJie jingJie, DeckIndex deckIndex)
    {
        SkillSetJingJieDetails d = new(jingJie, deckIndex);
        RunSkill template = SkillFromDeckIndex(deckIndex);
        SetSkillProcedure(RunSkill.FromChangeJingJie(template, jingJie), deckIndex);
        SkillSetJingJieNeuron.Invoke(d);
    }

    public void SetSkillProcedure(RunSkill template, DeckIndex deckIndex)
    {
        SetSkillDetails d = new(template, deckIndex);
        
        switch (deckIndex.Region)
        {
            case SkillRegion.Field:
                Home.GetSlot(deckIndex.Index).Skill = template.Clone();
                break;
            case SkillRegion.Hand:
                Hand.Replace(deckIndex.Index, template.Clone());
                break;
            case SkillRegion.Requirement:
            {
                RequireCell requireCell = Cell?.AsCell() as RequireCell;
                Assert.IsTrue(requireCell != null);
                requireCell.RequirementSlotList[deckIndex.Index].Skill = template.Clone();
                break;
            }
            case SkillRegion.Barter:
            {
                BarterCell barterCell = Cell?.AsCell() as BarterCell;
                Assert.IsTrue(barterCell != null);
                barterCell.LeftBucketItems[deckIndex.Index] = template.Clone();
                break;
            }
        }

        SetSkillNeuron.Invoke(d);
    }

    public void UpgradeAllSkillsToHuaShenProcedure()
    {
        TraversalDeckIndices().Do(deckIndex =>
        {
            RunSkill skill = SkillFromDeckIndex(deckIndex);
            if (skill == null)
                return;

            JingJie toJingJie = ((int)(skill.GetEntry().HighestJingJie)).ClampUpper(JingJie.HuaShen);
            RunManager.Instance.Environment.SkillSetJingJieProcedure(toJingJie, deckIndex);
        });
    }

    #endregion

    #region PanelOperations
    
    private RoomEnvironment _roomEnvironment;
    [NonSerialized] private ICellAdapter _cell;

    private RunState _runState;

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
    
    private void SelectedMapNodeWithRoomEntry(MapNode mapNode, RoomEntry roomEntry)
    {
        RunManager.Instance.Environment.ReceiveSignalProcedure(SelectedMapNodeSignal.FromMapNodeAndRoomEntry(mapNode, roomEntry));
    }
    
    public void ReceiveSignalProcedure(Signal signal)
    {
        if (_runState == RunState.Committed)
            return;

        if (_runState == RunState.MapSelecting && signal is SelectedMapNodeSignal selectedMapNodeSignal)
        {
            EnterRoomProcedure(selectedMapNodeSignal.MapNode, selectedMapNodeSignal.RoomEntry, 0);
            return;
        }

        if (_runState == RunState.InRoom)
        {
            _roomEnvironment.ReceiveSignal(signal);
            
            if (_roomEnvironment.IsFinished())
            {
                ExitRoomProcedure();
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

    public void EnterRoomProcedure(MapNode mapNode, RoomEntry roomEntry, int ladder)
    {
        _runState = RunState.InRoom;
        _roomEnvironment = RoomEnvironment.CreateRoom(mapNode, roomEntry, ladder);
        _roomEnvironment.Step();
        Cell = _roomEnvironment.CurrentCell;
    }

    public void ExitRoomProcedure()
    {
        _runState = RunState.MapSelecting;
        _roomEnvironment = null;
        Cell = null;
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

    public MenuDetails GetMenuDetailsFromMapNode(MapNode mapNode)
    {
        List<RoomOption> roomOptions = mapNode.GetRoomOptions();

        List<MenuOption> menuOptions = new List<MenuOption>();
        roomOptions.Do(roomOption =>
        {
            if (roomOption.RoomEntry == null)
                return;

            MenuOption menuOption = new(roomOption.Description, ClickAction);
            menuOptions.Add(menuOption);
            return;
            
            void ClickAction() => SelectedMapNodeWithRoomEntry(mapNode, roomOption.RoomEntry);
        });
        
        return new MenuDetails(menuOptions);
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
        EditorManager.Instance.Add(Home);
        int ladder = Map.GetCurrRoom().Ladder;
        Home.SetLadder(ladder);
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
