
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RunEnvironment : Addressable, RunClosureOwner, ISerializationCallbackReceiver
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
        GainSkillsNeuron = new();
        BuySkillNeuron = new();
        ExchangeSkillNeuron = new();
        GachaNeuron = new();
        SelectOptionNeuron = new();
        PanelChangedNeuron = new();
        DeckChangedNeuron = new();
        FieldChangedNeuron = new();
        GainMingYuanNeuron = new();
        LoseMingYuanNeuron = new();
        GainGoldNeuron = new();
        LoseGoldNeuron = new();
        GainDHealthNeuron = new();
        LoseDHealthNeuron = new();
    }

    public Neuron StartRunNeuron;
    public Neuron<EquipDetails> EquipNeuron;
    public Neuron<SwapDetails> SwapNeuron;
    public Neuron<UnequipDetails> UnequipNeuron;
    public Neuron<MergeDetails> MergeNeuron;
    public Neuron<GainSkillDetails> GainSkillNeuron;
    public Neuron<PickDiscoveredSkillDetails> PickDiscoveredSkillNeuron;
    public Neuron<GainSkillsDetails> GainSkillsNeuron;
    public Neuron<BuySkillDetails> BuySkillNeuron;
    public Neuron<ExchangeSkillDetails> ExchangeSkillNeuron;
    public Neuron<GachaDetails> GachaNeuron;
    public Neuron<SelectOptionDetails> SelectOptionNeuron;
    public Neuron<PanelChangedDetails> PanelChangedNeuron;
    public Neuron<DeckChangedDetails> DeckChangedNeuron;
    public Neuron FieldChangedNeuron;
    public Neuron<int> GainMingYuanNeuron;
    public Neuron<int> LoseMingYuanNeuron;
    public Neuron<int> GainGoldNeuron;
    public Neuron<int> LoseGoldNeuron;
    public Neuron<int> GainDHealthNeuron;
    public Neuron<int> LoseDHealthNeuron;
    
    #endregion

    #region Core
    
    [NonSerialized] private Memory _memory;
    [NonSerialized] private RunClosureDict _closureDict;
    [NonSerialized] private Dirty<StageResult> _simulateResult;
    [NonSerialized] private PanelDescriptor _panel;
    [NonSerialized] private RunEntity _away;
    [NonSerialized] private bool _awayIsDummy;

    [NonSerialized] private DateTime _startTime;
    [SerializeField] private TimeSpan _loadedTime;
    [SerializeField] private TimeSpan _runFinishedTime;
    
    [SerializeReference] private RunConfig _config;
    [SerializeField] private JingJie _jingJie;
    [SerializeReference] private Map _map;
    [SerializeReference] private SkillPool _skillPool;
    [SerializeReference] private SkillInventory _hand;
    [SerializeField] private BoundedInt _gold;
    [SerializeReference] private RunEntity _home;
    [SerializeField] private RunResult _result;

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
            { "ActivePanel",           GetActivePanel },
        };
        
        _startTime = DateTime.Now;
        
        InitNeurons();

        _config = config;
        _map = new(_config.MapEntry);
        _skillPool = new();
        _hand = new();
        _gold = new(0);
        _result = new();

        _memory = new();
        _closureDict = new();
        _simulateResult = new(Simulate);

        SetHome(RunEntity.Default());
        SetAway(null);
        
        FieldChangedNeuron.Add(_simulateResult.SetDirty);
        DeckChangedNeuron.Add(GuideProcedure);
    }

    public static RunEnvironment FromConfig(RunConfig config)
        => new(config);

    #endregion

    #region Accessors

    public RunConfig GetRunConfig() => _config;
    public Map Map => _map;
    public RunEntity Home => _home;
    public RunEntity Away => _away;
    public JingJie JingJie => _jingJie;
    public Pool<SkillEntry> SkillPool => _skillPool;
    public SkillInventory Hand => _hand;
    public void SendEvent(int eventId, ClosureDetails closureDetails) => _closureDict.SendEvent(eventId, closureDetails);
    public StageResult GetSimulateResult() => _simulateResult.Value;
    public RunResult GetResult() => _result;
    public PanelDescriptor GetActivePanel() => Panel;
    public TimeSpan GetRunfinishedTime() => _runFinishedTime;

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
    }

    public bool AwayIsDummy() => _awayIsDummy;

    public void Register()
    {
        RegisterList(_config.CharacterProfile.GetEntry()._runClosures);

        DifficultyEntry difficultyEntry = _config.DifficultyProfile.GetEntry();
        RegisterList(difficultyEntry._runClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            RegisterList(additionalDifficultyEntry._runClosures);
    }

    private void RegisterList(RunClosure[] list)
    {
        list.Do(e => _closureDict.Register(this, e));
    }

    public void Unregister()
    {
        UnregisterList(_config.CharacterProfile.GetEntry()._runClosures);

        DifficultyEntry difficultyEntry = _config.DifficultyProfile.GetEntry();
        UnregisterList(difficultyEntry._runClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.InheritedDifficulties)
            UnregisterList(additionalDifficultyEntry._runClosures);
    }

    private void UnregisterList(RunClosure[] list)
    {
        list.Do(e => _closureDict.Unregister(this, e));
    }

    public BoundedInt GetGold()
        => _gold;

    public MingYuan GetMingYuan()
        => _home.GetMingYuan();

    public RunSkill GetSkillAtDeckIndex(DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            return Home.GetSlot(deckIndex.Index).Skill;
        else
            return Hand[deckIndex.Index];
    }

    public DeckIndex? GetDeckIndexOfSkill(RunSkill runSkill)
    {
        SkillSlot skillSlot = runSkill.GetSkillSlot();
        if (skillSlot != null)
            return DeckIndex.FromField(skillSlot.GetIndex());

        if (Hand.Contains(runSkill))
            return DeckIndex.FromHand(Hand.IndexOf(runSkill));

        return null;
    }

    public bool FindDeckIndex(out DeckIndex result, SkillEntryDescriptor descriptor, bool excludingField = false, bool excludingHand = false, DeckIndex[] omit = null)
    {
        omit ??= Array.Empty<DeckIndex>();
        
        result = default;
        
        foreach (DeckIndex deckIndex in TraversalDeckIndices(excludingField, excludingHand))
        {
            if (omit.Contains(deckIndex))
                continue;
            RunSkill skill = GetSkillAtDeckIndex(deckIndex);
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

    public void SetGuideToFinish()
    {
        GetActivePanel().SetGuideToFinish();
    }

    public string GetJingJieHintText()
    {
        return "有五个境界：\n练气，筑基\n金丹，元婴\n化神";
    }

    public string GetDifficultyHintText()
    {
        return _config.DifficultyProfile.GetEntry().InheritedDescription;
    }

    public void SetVariable<T>(string key, T value) => _memory.SetVariable(key, value);
    public T TryGetVariable<T>(string key, T defaultValue) => _memory.TryGetVariable(key, defaultValue);
    public void PerformOperation<T>(string key, T defaultValue, Func<T, T> operation) => _memory.PerformOperation(key, defaultValue, operation);

    public bool IsFinalJingJie()
        => _jingJie == _config.DifficultyProfile.GetEntry().FinalJingJie;

    #endregion

    #region Procedures

    public void StartRunProcedure(StartRunDetails d)
    {
        InitSkillPool();
        
        SetJingJieProcedure(Map.GetEntry()._envJingJie);
        _home.SetSlotCount(Map.GetEntry()._slotCount);
        SetDGoldProcedure(Map.GetEntry()._gold);
        
        DrawSkillsProcedure(new(jingJie: Map.GetEntry()._skillJingJie, count: Map.GetEntry()._skillCount));

        Map.Init();
        InitPanel();
        
        _closureDict.SendEvent(RunClosureDict.START_RUN, d);
        StartRunNeuron.Invoke();
    }

    public void ContinueRunProcedure(ContinueRunDetails d)
    {
        SetJingJieProcedure(_jingJie);
        
        InitPanel();
    }

    private void InitSkillPool()
    {
        4.Do(_ => SkillPool.Populate(Encyclopedia.SkillCategory.Traversal.FilterObj(e => e.WithinPool)));
    }

    public void NextJingJieProcedure()
        => SetJingJieProcedure(JingJie + 1);
    
    public void SetJingJieProcedure(JingJie toJingJie)
        => SetJingJieProcedure(new SetJingJieDetails(JingJie, toJingJie));
    
    private void SetJingJieProcedure(SetJingJieDetails d)
    {
        _closureDict.SendEvent(RunClosureDict.WIL_SET_JINGJIE, d);
        if (d.Cancel)
            return;

        _jingJie = d.ToJingJie;
        
        // move to ascension procedure
        int dHealth = RunEntity.HealthFromJingJie[d.ToJingJie] - RunEntity.HealthFromJingJie[d.FromJingJie];
        SetDHealthProcedure(new SetDHealthDetails(dHealth));
        
        _home.SetJingJie(d.ToJingJie);
        AudioManager.Play(Encyclopedia.AudioFromJingJie(d.ToJingJie));

        _closureDict.SendEvent(RunClosureDict.DID_SET_JINGJIE, d);
        
        CanvasManager.Instance.RunCanvas.TopBar.Refresh();
    }

    public PanelDescriptor LegacyReceiveSignalProcedure(Signal signal)
    {
        Guide guide = Panel.GetGuideDescriptor();
        guide?.ReceiveSignal(Panel, signal);

        if (signal is DeckChangedSignal)
            return Panel;
        
        PanelDescriptor panelDescriptor = Panel.ReceiveSignal(signal);
        bool runIsUnfinished = _result.GetState() == RunResult.RunResultState.Unfinished;

        if (!runIsUnfinished)
        {
            Panel = new RunResultPanelDescriptor(_result);
            return Panel;
        }
        
        if (panelDescriptor != null) // descriptor of panel descriptor
        {
            Panel = panelDescriptor;
            return Panel;
        }
        
        if (Map.IsAboutToFinish())
        {
            CommitRunProcedure(RunResult.RunResultState.Victory);
            return Panel;
        }
        
        if (Map.IsLastStep())
        {
            Map.NextLevel();
            return Panel;
        }
        
        Map.NextStep();
        return Panel;
    }

    public void GuideProcedure(DeckChangedDetails d)
    {
        Guide guide = Panel.GetGuideDescriptor();
        guide?.ReceiveSignal(Panel, new DeckChangedSignal(d.FromIndex, d.ToIndex));
    }

    private StageResult Simulate()
    {
        PlacementProcedure();
        FormationProcedure();

        return StageEnvironment.CalcSimulateResult(StageConfig.ForSimulate(_home, _away, _config));
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

    public MergePreresult GetMergePreresult(RunSkill lhs, RunSkill rhs)
    {
        JingJie playerJingJie = _home.GetJingJie();
        
        var tuple = MergePreresult.MergeRules.FirstObj(tuple => tuple.Item1(lhs, rhs, playerJingJie));
        if (tuple != null)
            return tuple.Item2(lhs, rhs, playerJingJie);

        return MergePreresult.FromDefault(lhs, rhs, playerJingJie);
    }

    public void MergeProcedure(MergeDetails d)
    {
        _closureDict.SendEvent(RunClosureDict.WIL_MERGE, d);

        if (d.Cancel)
            return;

        bool success = InnerMerge(d);
        if (!success)
            return;
        
        _closureDict.SendEvent(RunClosureDict.DID_MERGE, d);
        
        DeckChangedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
        MergeNeuron.Invoke(d);
    }

    private bool InnerMerge(MergeDetails d)
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
        DeckIndex rhsDeckIndex = rhs.ToDeckIndex();
        
        if (MergePreresult.IsCongruent(lhs, rhs, playerJingJie))
        {
            rhs.JingJie = (rJingJie + 2).ClampUpper(rEntry.HighestJingJie);
            Hand.Remove(d.Lhs);
            return true;
        }
        
        if (MergePreresult.IsSameName(lhs, rhs, playerJingJie))
        {
            rhs.JingJie = (Mathf.Max(lJingJie, rJingJie) + 1).ClampUpper(rEntry.HighestJingJie);
            Hand.Remove(d.Lhs);
            return true;
        }

        bool valid = rhs.GetJingJie() <= playerJingJie && lhs.GetJingJie() <= playerJingJie;
        if (!valid)
            return false;
        
        if (MergePreresult.IsSameWuXing(lhs, rhs, playerJingJie))
        {
            DeckIndex? refDeckIndex = rhsDeckIndex;
            SkillEntryDescriptor skillEntryDescriptor = new(
                pred: skillEntry => skillEntry != lEntry && skillEntry != rEntry,
                wuXing: rWuXing,
                jingJie: rJingJie + 1);
            InnerDrawCreateAdd(skillEntryDescriptor, ref refDeckIndex);
            Hand.Remove(d.Lhs);
            return true;
        }
        
        if (MergePreresult.IsXiangShengWuXing(lhs, rhs, playerJingJie))
        {
            DeckIndex? refDeckIndex = rhsDeckIndex;
            SkillEntryDescriptor skillEntryDescriptor = new(
                wuXing: WuXing.XiangShengNext(lWuXing, rWuXing).Value,
                jingJie: rJingJie + 1);
            InnerDrawCreateAdd(skillEntryDescriptor, ref refDeckIndex);
            Hand.Remove(d.Lhs);
            return true;
        }
        
        if (MergePreresult.IsSameJingJie(lhs, rhs, playerJingJie))
        {
            DeckIndex? refDeckIndex = rhsDeckIndex;
            SkillEntryDescriptor skillEntryDescriptor = new(
                pred: skillEntry => skillEntry.WuXing.HasValue && skillEntry.WuXing != lWuXing &&
                                    skillEntry.WuXing != rWuXing,
                jingJie: rJingJie + 1);
            InnerDrawCreateAdd(skillEntryDescriptor, ref refDeckIndex);
            Hand.Remove(d.Lhs);
            return true;
        }
        
        if (MergePreresult.IsHuaShenReroll(lhs, rhs, playerJingJie))
        {
            DeckIndex? refDeckIndex = rhsDeckIndex;
            SkillEntryDescriptor skillEntryDescriptor = new(
                pred: skillEntry => skillEntry.WuXing.HasValue && skillEntry.WuXing != lWuXing &&
                                    skillEntry.WuXing != rWuXing,
                jingJie: rJingJie);
            InnerDrawCreateAdd(skillEntryDescriptor, ref refDeckIndex);
            Hand.Remove(d.Lhs);
            return true;
        }
        
        return false;
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

        DeckChangedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
        FieldChangedNeuron.Invoke();
        EquipNeuron.Invoke(d);
    }

    public void SwapProcedure(SwapDetails d)
    {
        d.IsReplace = d.ToSlot.Skill != null;
        RunSkill temp = d.FromSlot.Skill;
        d.FromSlot.Skill = d.ToSlot.Skill;
        d.ToSlot.Skill = temp;

        DeckChangedNeuron.Invoke(new(d.FromDeckIndex, d.ToDeckIndex));
        FieldChangedNeuron.Invoke();
        SwapNeuron.Invoke(d);
    }

    public void UnequipProcedure(UnequipDetails d)
    {
        RunSkill toUnequip = d.SkillSlot.Skill;
        Hand.Add(toUnequip);
        d.SkillSlot.Skill = null;
        
        DeckChangedNeuron.Invoke(new(d.FromDeckIndex, DeckIndex.FromHand(Hand.Count() - 1)));
        FieldChangedNeuron.Invoke();
        UnequipNeuron.Invoke(d);
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

    public void ClearDeck()
    {
        Hand.Clear();
        Home.TraversalCurrentSlots().Do(s => s.Skill = null);
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
        
        _closureDict.SendEvent(RunClosureDict.WIL_SET_D_MINGYUAN, d);

        if (d.Cancel)
            return;

        GetMingYuan().Curr += d.Value;
        _closureDict.SendEvent(RunClosureDict.DID_SET_D_MINGYUAN, d);
        if (d.Value >= 0)
            GainMingYuanNeuron.Invoke(d.Value);
        else
            LoseMingYuanNeuron.Invoke(d.Value);

        // register this as a defeat check
        if (GetMingYuan().Curr <= 0)
            CommitRunProcedure(RunResult.RunResultState.Defeat);
    }

    public void SetDGoldProcedure(int value)
        => SetDGoldProcedure(new SetDGoldDetails(value));
    
    private void SetDGoldProcedure(SetDGoldDetails d)
    {
        if (d.Value == 0)
            return;
        
        _closureDict.SendEvent(RunClosureDict.WILL_SET_D_GOLD, d);

        if (d.Cancel)
            return;

        _gold.Curr += d.Value;
        _closureDict.SendEvent(RunClosureDict.DID_SET_D_GOLD, d);
        if (d.Value >= 0)
            GainGoldNeuron.Invoke(d.Value);
        else
            LoseGoldNeuron.Invoke(d.Value);
    }

    public void SetDHealthProcedure(int value)
        => SetDHealthProcedure(new SetDHealthDetails(value));
    
    private void SetDHealthProcedure(SetDHealthDetails d)
    {
        if (d.Value == 0)
            return;
        
        _closureDict.SendEvent(RunClosureDict.WILL_SET_DDHEALTH, d);

        if (d.Cancel)
            return;

        _home.SetDHealth(d.Value);
        _closureDict.SendEvent(RunClosureDict.DID_SET_DDHEALTH, d);
        if (d.Value >= 0)
            GainDHealthNeuron.Invoke(d.Value);
        else
            LoseDHealthNeuron.Invoke(d.Value);
    }

    public void SetMaxMingYuanProcedure(int value)
        => SetMaxMingYuanProcedure(new SetMaxMingYuanDetails(value));
    
    private void SetMaxMingYuanProcedure(SetMaxMingYuanDetails d)
    {
        _closureDict.SendEvent(RunClosureDict.WILL_SET_MAX_MINGYUAN, d);

        if (d.Cancel)
            return;

        int diff = GetMingYuan().Curr - d.Value;
        if (diff > 0)
            SetDMingYuanProcedure(diff);

        GetMingYuan().UpperBound = d.Value;

        _closureDict.SendEvent(RunClosureDict.DID_SET_MAX_MINGYUAN, d);
    }

    public void DiscoverSkillProcedure(DiscoverSkillDetails d)
    {
        _closureDict.SendEvent(RunClosureDict.WILL_DISCOVER_SKILL, d);

        List<SkillEntry> entries = InnerDrawSkills(d.Descriptor);
        d.Skills.AddRange(entries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, d.PreferredJingJie)));

        _closureDict.SendEvent(RunClosureDict.DID_DISCOVER_SKILL, d);
    }

    public void ExitShopProcedure()
    {
        ReceiveSignalProcedure(new ExitShopSignal());
    }

    public void SelectOptionProcedure(SelectOptionDetails d)
    {
        SelectOptionNeuron.Invoke(d);
        Signal signal = new SelectedOptionSignal(d.SelectedIndex);
        ReceiveSignalProcedure(signal);
    }
    
    #endregion

    #region SkillRelatedProcedures
    
    public SkillEntry InnerDrawSkill(SkillEntryDescriptor descriptor)
    {
        SkillPool.TryPopItem(out SkillEntry skillEntry, descriptor.Contains);
        SkillPool.Shuffle();
        skillEntry ??= Encyclopedia.SkillCategory[0];
        return skillEntry;
    }
    
    private RunSkill InnerCreateSkill(SkillEntry skillEntry, JingJie? preferredJingJie = null)
    {
        JingJie jingJie = Mathf.Clamp(preferredJingJie ?? JingJie.LianQi, skillEntry.LowestJingJie, skillEntry.HighestJingJie);
        return RunSkill.FromEntryJingJie(skillEntry, jingJie);
    }

    private void InnerAddSkill(RunSkill skill, ref DeckIndex? preferredDeckIndex)
    {
        if (!preferredDeckIndex.HasValue)
        {
            Hand.Add(skill);
            preferredDeckIndex = DeckIndex.FromHand(Hand.Count() - 1);
            return;
        }
        
        DeckIndex deckIndex = preferredDeckIndex.Value;
        if (deckIndex.InField)
        {
            Home.GetSlot(deckIndex.Index).Skill = skill;
            return;
        }
        
        Hand.Replace(deckIndex.Index, skill);
    }
    
    private void InnerCreateAdd(SkillEntry skillEntry, JingJie? preferredJingJie, ref DeckIndex? preferredDeckIndex)
        => InnerAddSkill(InnerCreateSkill(skillEntry, preferredJingJie), ref preferredDeckIndex);
    
    public void InnerDrawCreateAdd(SkillEntryDescriptor descriptor, ref DeckIndex? preferredDeckIndex)
        => InnerAddSkill(InnerCreateSkill(InnerDrawSkill(descriptor), descriptor.JingJie), ref preferredDeckIndex);
    
    public void AddSkillProcedure(SkillEntry skillEntry, JingJie? preferredJingJie = null, DeckIndex? preferredDeckIndex = null)
    {
        RunSkill skill = InnerCreateSkill(skillEntry, preferredJingJie);
        DeckIndex? refDeckIndex = preferredDeckIndex;
        InnerAddSkill(skill, ref refDeckIndex);
        GainSkillNeuron.Invoke(new GainSkillDetails(refDeckIndex.Value, skill));
    }
    
    public void DrawSkillProcedure(SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null)
    {
        SkillEntry skillEntry = InnerDrawSkill(descriptor);
        RunSkill skill = InnerCreateSkill(skillEntry, descriptor.JingJie);
        DeckIndex? refDeckIndex = preferredDeckIndex;
        InnerAddSkill(skill, ref refDeckIndex);
        GainSkillNeuron.Invoke(new GainSkillDetails(refDeckIndex.Value, skill));
    }
    
    public List<SkillEntry> InnerDrawSkills(SkillEntryCollectionDescriptor d)
    {
        List<SkillEntry> toRet = new();
        
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

        SkillPool.Shuffle();
        return toRet;
    }

    public void DrawSkillsProcedure(SkillEntryCollectionDescriptor descriptor)
    {
        List<SkillEntry> entries = InnerDrawSkills(descriptor);
        
        int start = Hand.Count();
        DeckIndex[] indices = new DeckIndex[descriptor.Count];
        for (int i = 0; i < descriptor.Count; i++)
        {
            SkillEntry e = entries[i];
            
            RunSkill skill = InnerCreateSkill(e, descriptor.JingJie);
            Hand.Add(skill);

            indices[i] = DeckIndex.FromHand(start + i);
        }
        
        GainSkillsNeuron.Invoke(new GainSkillsDetails(indices));
    }
    
    public void PickDiscoveredSkillProcedure(PickDiscoveredSkillDetails d)
    {
        RunSkill skill = InnerCreateSkill(d.Skill.Entry, d.Skill.JingJie);
        Hand.Add(skill);
        PickDiscoveredSkillNeuron.Invoke(d);
        ReceiveSignalProcedure(new PickDiscoveredSkillSignal(d.PickedIndex));
    }

    public void BuySkillProcedure(BuySkillDetails d)
    {
        RunSkill skill = InnerCreateSkill(d.Commodity.Skill.Entry, d.Commodity.Skill.JingJie);
        Hand.Add(skill);
        d.DeckIndex = DeckIndex.FromHand(Hand.Count() - 1);
        BuySkillNeuron.Invoke(d);
    }

    public void ExchangeSkillProcedure(ExchangeSkillDetails d)
    {
        ExchangeSkillNeuron.Invoke(d);
    }

    public void GachaProcedure(GachaDetails d)
    {
        RunSkill skill = InnerCreateSkill(d.SkillEntryDescriptor.Entry, d.SkillEntryDescriptor.JingJie);
        Hand.Add(skill);
        d.DeckIndex = DeckIndex.FromHand(Hand.Count() - 1);
        GachaNeuron.Invoke(d);
    }
    
    public void ConfirmSelectionsProcedure(List<SkillEntryDescriptor> descriptors)
    {
        Signal signal = new ConfirmSkillsSignal(descriptors);
        ReceiveSignalProcedure(signal);
    }

    public void ConfirmDeckSelectionsProcedure(List<DeckIndex> indices)
    {
        Signal signal = new ConfirmDeckSignal(indices);
        ReceiveSignalProcedure(signal);
    }

    public void RemoveSkillAtDeckIndexProcedure(DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            Home.GetSlot(deckIndex.Index).Skill = null;
        else
            Hand.RemoveAt(deckIndex.Index);
        
        // staging
        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
    }

    public void SetJingJieAtDeckIndexProcedure(JingJie jingJie, DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            _home.GetSlot(deckIndex.Index).Skill.JingJie = jingJie;
        else
            Hand[deckIndex.Index].JingJie = jingJie;
        
        // staging
        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
    }

    public void ReplaceSkillAtDeckIndexProcedure(RunSkill template, DeckIndex deckIndex)
    {
        if (deckIndex.InField)
            Home.GetSlot(deckIndex.Index).Skill = template.Clone();
        else
            Hand.Replace(deckIndex.Index, template.Clone());
        
        // staging
        CanvasManager.Instance.RunCanvas.DeckPanel.Refresh();
    }

    #endregion

    #region Profile

    public void PrintJson()
    {
        string json = JsonUtility.ToJson(this, true);
        Debug.Log(json);
        GUIUtility.systemCopyBuffer = json;
    }

    #endregion

    #region PanelOperations
    
    public PanelDescriptor Panel
    {
        get => _panel;
        set
        {
            if (_panel == value)
                return;
            _panel?.Exit();
            _panel = value;
            _panel?.Enter();
        }
    }

    private bool PanelIsFinished(PanelDescriptor panel)
        => panel == null;
    
    public void ReceiveSignalProcedure(Signal signal)
    {
        if (signal is DeckChangedSignal)
            return;

        if (RunIsFinished())
            return;

        PanelDescriptor panel = Panel.ReceiveSignal(signal);
        
        if (PanelIsFinished(panel))
        {
            if (Map.IsAboutToFinish())
            {
                CommitRunProcedure(RunResult.RunResultState.Victory);
                return;
            }
            else
            {
                Map.Step();
                
                bool cond1 = Map.GetCurrRoom().GetDescriptor() is SuccessRoomDescriptor;
                bool cond2 = Map.GetCurrRoom().GetDescriptor() is AscensionRoomDescriptor && IsFinalJingJie();
                if (cond1 || cond2)
                {
                    CommitRunProcedure(RunResult.RunResultState.Victory);
                    return;
                }
                
                panel = Map.CreatePanelFromCurrRoom();
            }
        }
        
        PanelChangedDetails panelChangedDetails = new(Panel, panel);
        Panel = panel;
        PanelChangedNeuron.Invoke(panelChangedDetails);
    }

    public void CommitRunProcedure(RunResult.RunResultState state)
    {
        _result.SetState(state);

        _runFinishedTime = _loadedTime + (DateTime.Now - _startTime);
        
        RunResultPanelDescriptor resultPanel = new RunResultPanelDescriptor(_result);
        
        PanelChangedDetails panelChangedDetails = new(Panel, resultPanel);
        Panel = resultPanel;
        PanelChangedNeuron.Invoke(panelChangedDetails);
    }

    public bool RunIsFinished()
        => _result.GetState() != RunResult.RunResultState.Unfinished;

    private void InitPanel()
    {
        Panel = Map.CreatePanelFromCurrRoom();
    }

    #endregion

    #region Serialization

    public void SaveProcedure()
    {
        _loadedTime += DateTime.Now - _startTime;
        _startTime = DateTime.Now;
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Config",                () => _config },
            { "Home",                  () => _home },
            { "Map",                   () => _map },
            { "Hand",                  () => _hand },
            { "ActivePanel",           GetActivePanel },
        };
        
        _startTime = DateTime.Now;
        
        InitNeurons();

        _memory = new();
        _closureDict = new();
        _simulateResult = new(Simulate);
        
        SetHome(_home);
        SetAway(null);
        
        FieldChangedNeuron.Add(_simulateResult.SetDirty);
        DeckChangedNeuron.Add(GuideProcedure);
    }

    #endregion
}
