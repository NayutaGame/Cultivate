
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class RunEnvironment : Addressable, RunClosureOwner
{
    #region Memory

    private Dictionary<string, object> _memory;

    public void SetVariable<T>(string key, T value)
        => _memory[key] = value;

    public T GetVariable<T>(string key)
        => (T)_memory[key];

    #endregion

    #region Neurons
    
    public Neuron<EquipDetails> EquipNeuron = new();
    public Neuron<SwapDetails> SwapNeuron = new();
    public Neuron<UnequipDetails> UnequipNeuron = new();
    public Neuron<MergeDetails> MergeNeuron = new();
    public Neuron<GainSkillDetails> GainSkillNeuron = new();
    public Neuron<PickDiscoveredSkillDetails> PickDiscoveredSkillNeuron = new();
    public Neuron<GainSkillsDetails> GainSkillsNeuron = new();
    public Neuron<BuySkillDetails> BuySkillNeuron = new();
    public Neuron<ExchangeSkillDetails> ExchangeSkillNeuron = new();
    public Neuron<GachaDetails> GachaNeuron = new();
    public Neuron<SelectOptionDetails> SelectOptionNeuron = new();
    public Neuron<PanelChangedDetails> PanelChangedNeuron = new();
    public Neuron<DeckChangedDetails> DeckChangedNeuron = new();
    public Neuron FieldChangedNeuron = new();
    
    public Neuron<int> GainMingYuanNeuron = new();
    public Neuron<int> LoseMingYuanNeuron = new();
    public Neuron<int> GainGoldNeuron = new();
    public Neuron<int> LoseGoldNeuron = new();
    public Neuron<int> GainDHealthNeuron = new();
    public Neuron<int> LoseDHealthNeuron = new();
    
    #endregion

    #region Core
    
    private RunConfig _config;
    private RunEntity _home; public RunEntity Home => _home;
    private RunEntity _away; public RunEntity Away => _away;
    public Map Map { get; private set; }
    public Pool<SkillEntry> SkillPool;
    public SkillInventory Hand { get; private set; }
    private BoundedInt _gold;
    private JingJie _jingJie;
    public JingJie JingJie => _jingJie;
    private RunClosureDict _closureDict;
    public RunClosureDict ClosureDict => _closureDict;

    public Dirty<StageResult> SimulateResult;
    public StageResult GetSimulateResult() => SimulateResult.Value;
    public RunResult Result { get; }
    private RunResultPanelDescriptor _runResultPanelDescriptor;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    private RunEnvironment(RunConfig config)
    {
        _accessors = new()
        {
            { "Config",                () => _config },
            { "Home",                  () => _home },
            { "Map",                   () => Map },
            { "Hand",                  () => Hand },
            { "ActivePanel",           GetActivePanel },
        };

        _gold = new(0);
        _config = config;

        _memory = new();

        SetHome(RunEntity.Default());
        SetAway(null);

        Map = new(_config.MapEntry);
        SkillPool = new();
        Hand = new();
        _closureDict = new();

        Result = new RunResult();
        SimulateResult = new(Simulate);

        FieldChangedNeuron.Add(SimulateResult.SetDirty);
        DeckChangedNeuron.Add(GuideProcedure);
    }

    public static RunEnvironment FromConfig(RunConfig config)
        => new(config);

    #endregion

    #region Accessors
    
    public PanelDescriptor GetActivePanel()
        => _runResultPanelDescriptor ?? Map.Panel;

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

    [NonSerialized] private bool _awayIsDummy;
    public bool AwayIsDummy() => _awayIsDummy;

    public void Register()
    {
        RegisterList(_config.CharacterProfile.GetEntry()._runClosures);

        DifficultyEntry difficultyEntry = _config.DifficultyProfile.GetEntry();
        RegisterList(difficultyEntry._runClosures);
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
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
        foreach (var additionalDifficultyEntry in difficultyEntry.AdditionalDifficulties)
            UnregisterList(additionalDifficultyEntry._runClosures);
    }

    private void UnregisterList(RunClosure[] list)
    {
        list.Do(e => _closureDict.Unregister(this, e));
    }

    public BoundedInt GetGold()
        => _gold;

    public MingYuan GetMingYuan()
        => _home.MingYuan;

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
            return DeckIndex.FromField(skillSlot.Index);

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
                yield return new DeckIndex(true, slot.Index);
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

    #endregion

    #region Procedures

    public void StartRunProcedure(RunDetails d)
    {
        InitSkillPool();
        
        SetJingJieProcedure(Map.Entry._envJingJie);
        _home.SetSlotCount(Map.Entry._slotCount);
        SetDGoldProcedure(Map.Entry._gold);
        
        DrawSkillsProcedure(new(jingJie: Map.Entry._skillJingJie, count: Map.Entry._skillCount));

        Map.Init();
        
        _closureDict.SendEvent(RunClosureDict.START_RUN, d);
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
        _home.SetBaseHealth(RunEntity.BaseHealthFromJingJie[d.ToJingJie]);
        
        _home.SetJingJie(d.ToJingJie);
        AudioManager.Play(Encyclopedia.AudioFromJingJie(d.ToJingJie));

        _closureDict.SendEvent(RunClosureDict.DID_SET_JINGJIE, d);
        
        CanvasManager.Instance.RunCanvas.TopBar.Refresh();
    }

    public PanelDescriptor LegacyReceiveSignalProcedure(Signal signal)
    {
        Guide guide = Map.Panel.GetGuideDescriptor();
        guide?.ReceiveSignal(Map.Panel, signal);

        if (signal is DeckChangedSignal)
            return Map.Panel;
        
        PanelDescriptor panelDescriptor = Map.Panel.ReceiveSignal(signal);
        bool runIsUnfinished = Result.State == RunResult.RunResultState.Unfinished;

        if (!runIsUnfinished)
        {
            _runResultPanelDescriptor = new RunResultPanelDescriptor(Result);
            Map.Panel = _runResultPanelDescriptor;
            return _runResultPanelDescriptor;
        }
        
        if (panelDescriptor != null) // descriptor of panel descriptor
        {
            Map.Panel = panelDescriptor;
            return Map.Panel;
        }
        
        if (Map.IsLastLevelAndLastStep())
        {
            CommitRunProcedure(RunResult.RunResultState.Victory);
            return Map.Panel;
        }
        
        if (Map.IsLastStep())
        {
            Map.NextLevel();
            return Map.Panel;
        }
        
        Map.NextStep();
        return Map.Panel;
    }

    public void GuideProcedure(DeckChangedDetails d)
    {
        Guide guide = Map.Panel.GetGuideDescriptor();
        guide?.ReceiveSignal(Map.Panel, new DeckChangedSignal(d.FromIndex, d.ToIndex));
    }
    
    public void ReceiveSignalProcedure(Signal signal)
    {
        if (signal is DeckChangedSignal)
            return;
        
        PanelDescriptor panelDescriptor = Map.Panel.ReceiveSignal(signal);
        bool runIsUnfinished = Result.State == RunResult.RunResultState.Unfinished;

        if (!runIsUnfinished)
        {
            _runResultPanelDescriptor = new RunResultPanelDescriptor(Result);

            PanelDescriptor fromPanel = Map.Panel;
            Map.Panel = _runResultPanelDescriptor;
            PanelChangedDetails panelChangedDetails = new(fromPanel, Map.Panel);
            PanelChangedNeuron.Invoke(panelChangedDetails);
            return;
        }
        
        if (panelDescriptor != null) // descriptor of panel descriptor
        {
            PanelDescriptor fromPanel = Map.Panel;
            Map.Panel = panelDescriptor;
            PanelChangedDetails panelChangedDetails = new(fromPanel, Map.Panel);
            PanelChangedNeuron.Invoke(panelChangedDetails);
            return;
        }
        
        if (Map.IsLastLevelAndLastStep())
        {
            PanelDescriptor fromPanel = Map.Panel;
            CommitRunProcedure(RunResult.RunResultState.Victory);
            PanelChangedDetails panelChangedDetails = new(fromPanel, Map.Panel);
            PanelChangedNeuron.Invoke(panelChangedDetails);
            return;
        }
        
        if (Map.IsLastStep())
        {
            PanelDescriptor fromPanel = Map.Panel;
            Map.NextLevel();
            PanelChangedDetails panelChangedDetails = new(fromPanel, Map.Panel);
            PanelChangedNeuron.Invoke(panelChangedDetails);
            return;
        }
        
        {
            PanelDescriptor fromPanel = Map.Panel;
            Map.NextStep();
            PanelChangedDetails panelChangedDetails = new(fromPanel, Map.Panel);
            PanelChangedNeuron.Invoke(panelChangedDetails);
        }
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

    public void CommitRunProcedure(RunResult.RunResultState state)
    {
        Result.State = state;
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

        _home.MingYuan.Curr += d.Value;
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

    public void SetDDHealthProcedure(int value)
        => SetDDHealthProcedure(new SetDDHealthDetails(value));
    
    private void SetDDHealthProcedure(SetDDHealthDetails d)
    {
        if (d.Value == 0)
            return;
        
        _closureDict.SendEvent(RunClosureDict.WILL_SET_DDHEALTH, d);

        if (d.Cancel)
            return;

        _home.SetDHealth(_home.GetDHealth() + d.Value);
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

        int diff = _home.MingYuan.Curr - d.Value;
        if (diff > 0)
            SetDMingYuanProcedure(diff);

        _home.MingYuan.UpperBound = d.Value;

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
    
    #endregion

    #region SkillRelatedProcedures
    
    public SkillEntry InnerDrawSkill(SkillEntryDescriptor descriptor)
    {
        SkillPool.Shuffle();
        SkillPool.TryPopItem(out SkillEntry skillEntry, descriptor.Contains);
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

    public void SelectOptionProcedure(SelectOptionDetails d)
    {
        SelectOptionNeuron.Invoke(d);
        Signal signal = new SelectedOptionSignal(d.SelectedIndex);
        ReceiveSignalProcedure(signal);
    }

    #endregion
}
