
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
        DeckChangedNeuron = new();
        FieldChangedNeuron = new();
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
    }

    public Neuron StartRunNeuron;
    public Neuron<EquipDetails> EquipNeuron;
    public Neuron<SwapDetails> SwapNeuron;
    public Neuron<UnequipDetails> UnequipNeuron;
    public Neuron<MergeDetails> MergeNeuron;
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
    public Neuron<DeckChangedDetails> DeckChangedNeuron;
    public Neuron FieldChangedNeuron;
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
    
    #endregion

    #region Core
    
    [NonSerialized] private Memory _memory;
    [NonSerialized] private RunClosureDict _closureDict;
    [NonSerialized] private Dirty<StageResult> _simulateResult;
    [NonSerialized] private PanelDescriptor _panel;
    [NonSerialized] private RunEntity _away;
    [NonSerialized] private bool _awayIsDummy;

    [NonSerialized] private DateTime _startTime;
    [NonSerialized] private TimeSpan _loadedTime;
    [NonSerialized] private TimeSpan _runFinishedTime;
    [NonSerialized] private RunReport _runReport;

    [SerializeField] private double _miliseconds;

    [SerializeReference] private SerializableDictionary _intMemory;
    [SerializeReference] private RunConfig _config;
    [SerializeField] private JingJie _jingJie;
    [SerializeReference] private Map _map;
    [SerializeReference] private SkillPool _skillPool;
    [SerializeReference] private SkillInventory _hand;
    [SerializeField] private BoundedInt _gold;
    [SerializeReference] private RunEntity _home;
    [SerializeField] private RunResult _result;

    [SerializeReference] private List<AchievementEntry> _newlyUnlockedAchievements;

    [SerializeField] public bool IsLegit = true;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEnvironment(RunConfig config)
    {
        _accessors = new()
        {
            { "Config",                () => _config },
            { "Home",                  () => _home },
            { "Map",                   () => _map },
            { "Hand",                  () => _hand },
            { "ActivePanel",           GetPanel },
        };
        
        _startTime = DateTime.Now;
        
        InitNeurons();

        _intMemory = new();
        _config = config;
        _map = new(_config.MapEntry);
        _skillPool = new();
        _hand = new();
        _gold = new(0);
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
        
        FieldChangedNeuron.Add(_simulateResult.SetDirty);
        DeckChangedNeuron.Add(GuideProcedure);

        if (AppManager.Instance.AudienceIsTester())
        {
            _runReport = new RunReport(this);
        
            RoomChangedNeuron.Add(AppendRoomReport);
            EngageEnemyNeuron.Add(AppendBattleReport);
            PanelChangedNeuron.Add(AppendPickDiscoveredSkillReport);
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
    public RunEntity Home => _home;
    public RunEntity Away => _away;
    public JingJie JingJie => _jingJie;
    public SkillPool SkillPool => _skillPool;
    public SkillInventory Hand => _hand;
    public void SendEvent(int eventId, RunClosureDetails closureDetails) => _closureDict.SendEvent(eventId, closureDetails);
    public StageResult GetSimulateResult() => _simulateResult.Value;
    public RunResult GetResult() => _result;
    public Sprite GetCurrEventIllustration() => _map.GetCurrEventIllustration();
    public TimeSpan GetRunfinishedTime() => _runFinishedTime;
    public TimeSpan GetPassedTime() => _loadedTime + (DateTime.Now - _startTime);
    public RunReport GetRunReport() => _runReport;

    public void SetHome(RunEntity home)
    {
        _home?.EnvironmentChangedNeuron.Remove(FieldChangedNeuron);
        _home = home;
        _home?.EnvironmentChangedNeuron.Add(FieldChangedNeuron);
    }

    public void SetAway(RunEntity away)
    {
        _awayIsDummy = away == null;
        away ??= RunEntity.FromJingJieHealth(_home.GetJingJie(), 1000000);
        
        _away?.EnvironmentChangedNeuron.Remove(FieldChangedNeuron);
        _away = away;
        _away?.EnvironmentChangedNeuron.Add(FieldChangedNeuron);

        EngageEnemyDetails d = new(_awayIsDummy, _away);
        EngageEnemyNeuron.Invoke(d);
    }

    public bool AwayIsDummy() => _awayIsDummy;

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

    public BoundedInt GetGold()
        => _gold;

    public MingYuan GetMingYuan()
        => _home.GetMingYuan();

    public RunSkill SkillFromDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            return Home.GetSlot(deckIndex.Index).Skill;
        else
            return Hand[deckIndex.Index];
    }

    public SkillSlot SlotFromDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            return Home.GetSlot(deckIndex.Index);
        else
            return null;
    }

    public DeckIndex? DeckIndexFromSkill(RunSkill runSkill)
    {
        SkillSlot skillSlot = runSkill.GetSkillSlot();
        if (skillSlot != null)
            return DeckIndex.FromField(skillSlot.GetIndex());

        if (Hand.Contains(runSkill))
            return DeckIndex.FromHand(Hand.IndexOf(runSkill));

        return null;
    }

    public bool DeckIndexFromDescriptor(out DeckIndex result, SkillEntryDescriptor descriptor, bool excludingField = false, bool excludingHand = false, DeckIndex[] omit = null)
    {
        omit ??= Array.Empty<DeckIndex>();
        
        result = default;
        
        foreach (DeckIndex deckIndex in TraversalDeckIndices(excludingField, excludingHand))
        {
            if (omit.Contains(deckIndex))
                continue;
            RunSkill skill = SkillFromDeckIndex(deckIndex);
            if (skill != null && descriptor.Contains(skill))
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
                yield return new DeckIndex(true, slot.GetIndex());
        if (!excludingHand)
            for (int i = 0; i < RunManager.Instance.Environment.Hand.Count(); i++)
                yield return new DeckIndex(false, i);
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
        GetPanel().SetGuideToFinish();
    }

    public string GetJingJieHintText()
    {
        return "有五个境界：\n练气，筑基\n金丹，元婴\n化神";
    }

    public string GetDifficultyHintText()
    {
        return _config.DifficultyProfile.GetEntry().InheritedDescription;
    }

    public bool IsFinalJingJie()
        => _jingJie == _config.DifficultyProfile.GetEntry().FinalJingJie;

    public void RecordNewlyUnlockedAchievement(AchievementEntry achievementEntry)
    {
        _newlyUnlockedAchievements.Add(achievementEntry);
        AppManager.Instance.ProfileManager.SaveProcedureForAchievements(achievementEntry);
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

    public void ClearSlotResults()
    {
        _home.TraversalCurrentSlots().Do(s => s.ClearResults());
        _away.TraversalCurrentSlots().Do(s => s.ClearResults());
    }

    #endregion

    #region Procedures

    public void StartRunProcedure(StartRunDetails d)
    {
        InitSkillPool();

        MapEntry mapEntry = Map.GetEntry();
        
        SetJingJieProcedure(mapEntry._envJingJie);
        _home.SetSlotCount(mapEntry._slotCount);
        SetDGoldProcedure(mapEntry._gold);
        
        DrawSkillsProcedure(new(jingJie: mapEntry._skillJingJie, count: mapEntry._skillCount));
        
        mapEntry.OnStartRun(this);

        Profile profile = AppManager.Instance.ProfileManager.GetCurrProfile();
        Map.Init(profile, this);
        InitPanel();
        
        SendEvent(RunClosureDict.START_RUN, d);
        StartRunNeuron.Invoke();
        
        AppManager.Instance.ProfileManager.SaveProcedure(this);
    }

    public void ContinueRunProcedure(ContinueRunDetails d)
    {
        SetJingJieProcedure(_jingJie);
        
        InitPanel();
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

        int count = _home.GetSlotCount();
        int preservedCount = d.PreserveFirstDeplete ? 1 : 0;
        for (int i = 0; i < count; i++)
        {
            SkillSlot slot = _home.GetSlot(i);
            RunSkill skill = slot.Skill;
            
            if (skill == null)
                continue;
            
            bool depleted = skill.GetEntry().GetSkillTypeComposite().Contains(SkillType.Deplete);
            if (!depleted)
                continue;

            if (preservedCount > 0)
            {
                preservedCount--;
                continue;
            }

            Debug.Log($"Depleting skill: {skill.GetEntry().GetName()}");

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
        AudioManager.Play(Encyclopedia.AudioFromJingJie(d.ToJingJie));

        SendEvent(RunClosureDict.DID_JINGJIE_CHANGE, d);
        
        JingJieChangedNeuron.Invoke(d);
    }

    private StageResult Simulate()
    {
        PlacementProcedure();
        FormationProcedure();
        SecondPlacementProcedure();

        return StageEnvironment.CalcSimulateResult(StageConfig.ForSimulate(_home, _away, _config));
    }

    private void PlacementProcedure()
    {
        _home.PlacementProcedure();
        _away.PlacementProcedure();
    }

    private void FormationProcedure()
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

    private void SecondPlacementProcedure()
    {
        _home.SecondPlacementProcedure();
        _away.SecondPlacementProcedure();
    }

    public MergeTarget GetMergePreresult(RunSkill lhs, RunSkill rhs)
    {
        MergeDetails d = new(lhs, rhs);
        d.PlayerJingJie = _home.GetJingJie();
        d.IsDryRun = true;
        InnerMerge(d);

        return d.MergeTarget;
    }

    public void MergeProcedure(MergeDetails d)
    {
        d.PlayerJingJie = _home.GetJingJie();
        d.IsDryRun = false;
        
        // buggy behaviour, event is stateful
        SendEvent(RunClosureDict.WIL_MERGE, d);

        if (d.Cancel)
            return;

        if (d.Lhs.Borrowed || d.Rhs.Borrowed)
            return;
        
        InnerMerge(d);
        Assert.IsTrue(d.State != MergeDetails.MergeState.Continue);

        if (d.State == MergeDetails.MergeState.Cancel)
            return;

        if (d.IsDryRun)
            return;

        ExecuteMergeTarget(d);
        
        SendEvent(RunClosureDict.DID_MERGE, d);
        
        MergeNeuron.Invoke(d);
        DeckChangedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
    }

    private void InnerMerge(MergeDetails d)
    {
        ProcessOverridingRules(d);
        if (d.State != MergeDetails.MergeState.Continue)
            return;

        ProcessDefaultRules(d);
    }

    private void ProcessOverridingRules(MergeDetails d)
    {
        MergeRule lhsRule = d.Lhs.GetEntry().OverridingMergeRule;
        MergeRule rhsRule = d.Rhs.GetEntry().OverridingMergeRule;

        if (lhsRule.Order <= rhsRule.Order)
        {
            d.Src = d.Lhs;
            d.Tgt = d.Rhs;
            lhsRule.ProcessMerge(d);
            if (d.State != MergeDetails.MergeState.Continue)
                return;
                
            d.Src = d.Rhs;
            d.Tgt = d.Lhs;
            rhsRule.ProcessMerge(d);
        }
        else
        {
            d.Src = d.Rhs;
            d.Tgt = d.Lhs;
            rhsRule.ProcessMerge(d);
            if (d.State != MergeDetails.MergeState.Continue)
                return;
                
            d.Src = d.Lhs;
            d.Tgt = d.Rhs;
            lhsRule.ProcessMerge(d);
        }
    }

    private void ProcessDefaultRules(MergeDetails d)
    {
        foreach (MergeRule mergeRule in MergeRule.DefaultMergeRules)
        {
            if (d.State != MergeDetails.MergeState.Continue)
                break;
            mergeRule.ProcessMerge(d);
        }
    }

    private void ExecuteMergeTarget(MergeDetails d)
    {
        d.ExecuteSideEffects();
        
        Assert.IsFalse(d.MergeTarget is InvalidMergeTarget);
        
        d.MergeTarget.Execute(d, Hand);
    }

    public void EquipProcedure(EquipDetails d)
    {
        RunSkill toUnequip = d.SkillSlot.Skill;
        RunSkill toEquip = d.Skill;

        if (toUnequip == null)
        {
            d.IsReplace = false;
            Hand.Remove(toEquip);
        }
        else
        {
            d.IsReplace = true;
            Hand.Replace(toEquip, toUnequip);
        }
        
        d.SkillSlot.Skill = toEquip;

        EquipNeuron.Invoke(d);
        DeckChangedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
        FieldChangedNeuron.Invoke();
    }

    public void SwapProcedure(SwapDetails d)
    {
        d.IsReplace = d.ToSlot.Skill != null;
        RunSkill temp = d.FromSlot.Skill;
        d.FromSlot.Skill = d.ToSlot.Skill;
        d.ToSlot.Skill = temp;

        SwapNeuron.Invoke(d);
        DeckChangedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
        FieldChangedNeuron.Invoke();
    }

    public void UnequipProcedure(UnequipDetails d)
    {
        RunSkill toUnequip = d.SkillSlot.Skill;
        if (toUnequip == null)
            return;
        Hand.Add(toUnequip);
        d.SkillSlot.Skill = null;
        
        UnequipNeuron.Invoke(d);
        DeckChangedNeuron.Invoke(new(d.DeckIndex, DeckIndex.FromHand(Hand.Count() - 1)));
        FieldChangedNeuron.Invoke();
    }

    public void LegacyUnequipProcedure(SkillSlot slot, object _)
    {
        RunSkill toUnequip = slot.Skill;
        if (toUnequip == null)
            return;

        if (toUnequip is RunSkill runSkill)
        {
            Hand.Add(runSkill);

            slot.Skill = null;
        }
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
                b.Pick(entry);
                b.SingleCreate(s.Skill.GetJingJie());
                b.RecordDeckIndex(s.ToDeckIndex());
            }
            else
            {
                b.Pick(entry);
                b.SingleCreate(s.Skill.GetJingJie());
                b.RecordDeckIndex(new NextHandDeckIndexDefinition());
            }
        });
        
        b.Add();
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

    public void Combat()
    {
        StageEnvironment.Combat(StageConfig.ForCombat(_home, _away, _config));
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
        b.Draw(d.Descriptor);
        
        d.Skills.AddRange(b.DrawnSkillEntries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, d.PreferredJingJie)));

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

    #region SkillRelatedProcedures
    
    public void PickSkillProcedure(SkillEntry skillEntry, JingJie? preferredJingJie = null, DeckIndex? preferredDeckIndex = null)
    {
        GainSkillBuilder b = new();
        b.Pick(skillEntry);
        b.Create(preferredJingJie);
        if (preferredDeckIndex != null)
            b.RecordDeckIndex(preferredDeckIndex);
        else
            b.RecordDeckIndex(new NextHandDeckIndexDefinition());
        b.Add();
        b.Invoke();
    }
    
    public void DrawSkillProcedure(SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null)
    {
        GainSkillBuilder b = new();
        b.Draw(descriptor);
        b.Create(descriptor.JingJie);
        b.RecordDeckIndex(preferredDeckIndex);
        b.Add();
        b.Invoke();
    }

    public void DrawSkillsProcedure(SkillEntryCollectionDescriptor descriptor)
    {
        GainSkillBuilder b = new();
        b.Draw(descriptor);
        b.Create(descriptor.JingJie);
        b.Add();
        b.Invoke();
    }
    
    public void PickDiscoveredSkillProcedure(PickDiscoveredSkillDetails d)
    {
        GainSkillBuilder b = new();
        b.Pick(d.Skill.Entry);
        b.Create(d.Skill.JingJie);
        b.Add();
        
        d.CreatedSkill = b.CreatedSkills[0].Clone();
        PickDiscoveredSkillNeuron.Invoke(d);
        ReceiveSignalProcedure(new PickDiscoveredSkillSignal(d.PickedIndex));
    }

    public void BuySkillProcedure(BuySkillDetails d)
    {
        GainSkillBuilder b = new();
        b.Pick(d.Commodity.Skill.Entry);
        b.Create(d.Commodity.Skill.JingJie);
        b.Add();

        d.DeckIndex = b.PreferredDeckIndices[0].Reify();
        BuySkillNeuron.Invoke(d);
    }

    public void ExchangeSkillProcedure(ExchangeSkillDetails d)
    {
        ExchangeSkillNeuron.Invoke(d);
    }

    public void GachaProcedure(GachaDetails d)
    {
        GainSkillBuilder b = new();
        b.Pick(d.SkillEntryDescriptor.Entry);
        b.Create(d.SkillEntryDescriptor.JingJie);
        b.Add();

        d.DeckIndex = b.PreferredDeckIndices[0].Reify();
        GachaNeuron.Invoke(d);
    }
    
    public void ConfirmSelectionsProcedure(List<SkillEntryDescriptor> descriptors)
    {
        ReceiveSignalProcedure(new ConfirmSkillsSignal(descriptors));
    }

    public void ConfirmDeckSelectionsProcedure(List<DeckIndex> indices)
    {
        ReceiveSignalProcedure(new ConfirmDeckSignal(indices));
    }

    public void RemoveSkillProcedure(SkillEntryDescriptor descriptor)
    {
        bool isFound = DeckIndexFromDescriptor(out DeckIndex deckIndex, descriptor);
        if (isFound)
            RemoveSkillProcedure(deckIndex);
    }

    public void RemoveSkillProcedure(DeckIndex deckIndex)
    {
        RemoveSkillDetails d = new(deckIndex);
        
        if (deckIndex.InField)
            Home.GetSlot(deckIndex.Index).Skill = null;
        else
            Hand.RemoveAt(deckIndex.Index);

        RemoveSkillNeuron.Invoke(d);
    }

    public void SkillSetJingJieProcedure(JingJie jingJie, DeckIndex deckIndex)
    {
        SkillSetJingJieDetails d = new(jingJie, deckIndex);
        
        if (deckIndex.InField)
            _home.GetSlot(deckIndex.Index).Skill.JingJie = jingJie;
        else
            Hand[deckIndex.Index].JingJie = jingJie;

        SkillSetJingJieNeuron.Invoke(d);
    }

    public void ReplaceSkillProcedure(RunSkill template, DeckIndex deckIndex)
    {
        ReplaceSkillDetails d = new(template, deckIndex);
        
        if (deckIndex.InField)
            Home.GetSlot(deckIndex.Index).Skill = template.Clone();
        else
            Hand.Replace(deckIndex.Index, template.Clone());

        ReplaceSkillNeuron.Invoke(d);
    }

    #endregion

    #region PanelOperations
    
    public PanelDescriptor GetPanel() => _panel;
    public void SetPanel(PanelDescriptor panel)
    {
        if (_panel == panel)
            return;
        PanelChangedDetails panelChangedDetails = new(_panel, panel);
        
        if (_panel != null)
            SendEvent(RunClosureDict.WIL_CHANGE_PANEL, panelChangedDetails);
        _panel?.Exit();
        _panel = panel;
        _panel?.Enter();
        if (_panel != null)
            PanelChangedNeuron.Invoke(panelChangedDetails);
    }

    private bool PanelIsFinished(PanelDescriptor panel)
        => panel == null;
    
    public void ReceiveSignalProcedure(Signal signal)
    {
        if (RunIsFinished())
            return;

        PanelDescriptor panel = _panel.ReceiveSignal(signal);
        
        if (PanelIsFinished(panel))
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
                
                AppManager.Instance.ProfileManager.SaveProcedure(this);
                
                panel = Map.CreatePanelFromCurrRoom();
                
                if (levelChanged)
                    LevelChangedNeuron.Invoke();

                Room newRoom = Map.GetCurrRoom();
                RoomChangedNeuron.Invoke(new(oldRoom, newRoom));
            }
        }
        
        SetPanel(panel);
    }

    public void GuideProcedure(DeckChangedDetails d)
        => GuideProcedure(new DeckChangedSignal(d.FromIndex, d.ToIndex));

    public void GuideProcedure(Signal signal)
    {
        Guide guide = _panel.GetGuideDescriptor();
        guide?.ReceiveSignal(_panel, signal);
        if (guide != null)
            CanvasManager.Instance.RefreshGuide();
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
        if (RunIsFinished())
            return;

        _result.SetOutcome(state);
        _runFinishedTime = GetPassedTime();
        
        RunResultPanelDescriptor resultPanel = new RunResultPanelDescriptor(this);
        
        SetPanel(resultPanel);

        SendEvent(RunClosureDict.DID_COMMIT_RUN, new RunCommitDetails(this));
    }

    public bool RunIsFinished()
        => _result.GetOutcome() != RunResult.RunOutcome.InProgress;

    private void InitPanel()
    {
        SetPanel(Map.CreatePanelFromCurrRoom());
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
        if (!IsLegit)
            return;

        _accessors = new()
        {
            { "Config",                () => _config },
            { "Home",                  () => _home },
            { "Map",                   () => _map },
            { "Hand",                  () => _hand },
            { "ActivePanel",           GetPanel },
        };
        
        _startTime = DateTime.Now;
        _loadedTime = TimeSpan.FromMilliseconds(_miliseconds);
        
        InitNeurons();

        for (int i = 0; i < _newlyUnlockedAchievements.Count; i++)
        {
            AchievementEntry entry = _newlyUnlockedAchievements[i];
            entry = string.IsNullOrEmpty(entry.GetId()) ? null : Encyclopedia.AchievementCategory[entry.GetId()];
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
        foreach (RunSkill skill in Hand.Traversal())
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
        DiscoverSkillPanelDescriptor panelDescriptor = d.ToPanel as DiscoverSkillPanelDescriptor;
        if (panelDescriptor == null)
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
