
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

    public Neuron BattleChangedNeuron = new();
    private void BattleEnvironmentUpdateProcedure()
    {
        EnvironmentUpdateDetails d = new();
        _closureDict.SendEvent(RunClosureDict.WIL_UPDATE, d);
        SimulateProcedure();
        _closureDict.SendEvent(RunClosureDict.DID_UPDATE, d);
    }
    
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

    public StageResult SimulateResult;
    public RunResult Result { get; }
    private RunResultPanelDescriptor _runResultPanelDescriptor;

    public Neuron<AddSkillDetails> AddSkillNeuron = new();
    public Neuron<EquipDetails> EquipNeuron = new();
    public Neuron<SwapDetails> SwapNeuron = new();
    public Neuron<UnequipDetails> UnequipNeuron = new();
    
    public Neuron<GainSkillDetails> LegacyGainSkillNeuron = new();
    public Neuron<GainSkillsDetails> GainSkillsNeuron = new();
    public Neuron<PickDiscoveredSkillDetails> PickDiscoveredSkillNeuron = new();
    public Neuron<BuySkillDetails> BuySkillNeuron = new();
    public Neuron<GachaDetails> GachaNeuron = new();
    public Neuron<int> GainMingYuanNeuron = new();
    public Neuron<int> LoseMingYuanNeuron = new();
    public Neuron<int> GainGoldNeuron = new();
    public Neuron<int> LoseGoldNeuron = new();
    public Neuron<int> GainDHealthNeuron = new();
    public Neuron<int> LoseDHealthNeuron = new();
    // Audio

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

        BattleChangedNeuron.Add(BattleEnvironmentUpdateProcedure);
    }

    public static RunEnvironment FromConfig(RunConfig config)
        => new(config);

    #endregion

    #region Accessors
    
    public PanelDescriptor GetActivePanel()
        => _runResultPanelDescriptor ?? Map.Panel;

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
        
        LegacyDrawSkillsProcedure(new(jingJie: Map.Entry._skillJingJie, count: Map.Entry._skillCount));

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

    public PanelDescriptor ReceiveSignalProcedure(Signal signal)
    {
        // if (Map.CurrNode == null)
        //     return null;
        
        Guide guide = Map.Panel.GetGuideDescriptor();
        if (guide != null)
        {
            bool blocksSignal = guide.ReceiveSignal(Map.Panel, signal);
            if (blocksSignal)
                return Map.Panel;
        }
        
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
        
        _closureDict.SendEvent(RunClosureDict.WIL_MERGE, d);

        if (d.Cancel)
            return false;

        bool success = InnerMergeProcedure(d);
        if (!success)
            return false;
        
        _closureDict.SendEvent(RunClosureDict.DID_MERGE, d);
        return true;
    }

    private bool InnerMergeProcedure(MergeDetails d)
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
            LegacyDrawSkillProcedureNoAnimation(new(
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
            LegacyDrawSkillProcedureNoAnimation(new(
                    wuXing: WuXing.XiangShengNext(lWuXing, rWuXing).Value,
                    jingJie: rJingJie + 1),
                preferredDeckIndex: rhsDeckIndex);
            Hand.Remove(lhs);
            BattleChangedNeuron.Invoke();
            return true;
        }
        
        if (MergePreresult.IsSameJingJie(lhs, rhs, playerJingJie))
        {
            LegacyDrawSkillProcedureNoAnimation(new(
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
            LegacyDrawSkillProcedureNoAnimation(new(
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
    }

    public void SwapProcedure(SwapDetails d)
    {
        d.IsReplace = d.ToSlot.Skill != null;
        RunSkill temp = d.FromSlot.Skill;
        d.FromSlot.Skill = d.ToSlot.Skill;
        d.ToSlot.Skill = temp;
        SwapNeuron.Invoke(d);
    }

    public void UnequipProcedure(UnequipDetails d)
    {
        RunSkill toUnequip = d.SkillSlot.Skill;
        Hand.Add(toUnequip);
        d.SkillSlot.Skill = null;
        UnequipNeuron.Invoke(d);
    }
    
    public bool LegacyEquipProcedure(out bool isReplace, RunSkill toEquip, SkillSlot slot)
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

    public UnequipResult LegacyUnequipProcedure(SkillSlot slot, object _)
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

    public bool LegacySwapProcedure(out bool isReplace, SkillSlot fromSlot, SkillSlot toSlot)
    {
        isReplace = toSlot.Skill != null;
        RunSkill temp = fromSlot.Skill;
        fromSlot.Skill = toSlot.Skill;
        toSlot.Skill = temp;
        return true;
    }

    public void ClearDeck()
    {
        Hand.Clear();
        Home.TraversalCurrentSlots().Do(s => s.Skill = null);
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

        List<SkillEntry> entries = LegacyDrawSkills(d.Descriptor);
        d.Skills.AddRange(entries.Map(e => SkillEntryDescriptor.FromEntryJingJie(e, d.PreferredJingJie)));

        _closureDict.SendEvent(RunClosureDict.DID_DISCOVER_SKILL, d);
    }
    
    public SkillEntry LegacyDrawSkill(SkillEntryDescriptor descriptor)
    {
        SkillPool.Shuffle();
        SkillPool.TryPopItem(out SkillEntry skillEntry, descriptor.Contains);
        skillEntry ??= Encyclopedia.SkillCategory[0];
        return skillEntry;
    }

    public List<SkillEntry> LegacyDrawSkills(SkillEntryCollectionDescriptor d)
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
    
    private RunSkill CreateSkill(SkillEntry skillEntry, JingJie? preferredJingJie = null)
    {
        JingJie jingJie = Mathf.Clamp(preferredJingJie ?? JingJie.LianQi, skillEntry.LowestJingJie, skillEntry.HighestJingJie);
        return RunSkill.FromEntryJingJie(skillEntry, jingJie);
    }
    
    private void LegacyAddSkill(RunSkill skill, DeckIndex? preferredDeckIndex = null)
    {
        if (!preferredDeckIndex.HasValue)
        {
            int last = Hand.Count();
            Hand.Add(skill);
            LegacyGainSkillNeuron.Invoke(new GainSkillDetails(DeckIndex.FromHand(last), skill));
            return;
        }

        DeckIndex deckIndex = preferredDeckIndex.Value;
        if (deckIndex.InField)
            Home.GetSlot(deckIndex.Index).Skill = skill;
        else
            Hand.Replace(deckIndex.Index, skill);
        
        LegacyGainSkillNeuron.Invoke(new GainSkillDetails(deckIndex, skill));
    }

    private void LegacyAddSkillNoAnimation(RunSkill skill, DeckIndex? preferredDeckIndex = null)
    {
        if (!preferredDeckIndex.HasValue)
        {
            int last = Hand.Count();
            Hand.Add(skill);
            LegacyGainSkillNeuron.Invoke(new GainSkillDetails(DeckIndex.FromHand(last), skill));
            return;
        }

        DeckIndex deckIndex = preferredDeckIndex.Value;
        if (deckIndex.InField)
            Home.GetSlot(deckIndex.Index).Skill = skill;
        else
            Hand.Replace(deckIndex.Index, skill);
    }
    
    public void LegacyDrawSkillsProcedure(SkillEntryCollectionDescriptor descriptor)
    {
        int start = Hand.Count();

        DeckIndex[] indices = new DeckIndex[descriptor.Count];
        
        List<SkillEntry> entries = LegacyDrawSkills(descriptor);
        
        for (int i = 0; i < descriptor.Count; i++)
        {
            SkillEntry e = entries[i];
            
            RunSkill skill = CreateSkill(e, descriptor.JingJie);
            Hand.Add(skill);

            indices[i] = DeckIndex.FromHand(start + i);
        }
        
        GainSkillsNeuron.Invoke(new GainSkillsDetails(indices));
    }

    public void LegacyDrawSkillProcedure(SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null)
        => LegacyAddSkill(CreateSkill(LegacyDrawSkill(descriptor), descriptor.JingJie), preferredDeckIndex);
    
    private void LegacyDrawSkillProcedureNoAnimation(SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null)
        => LegacyAddSkillNoAnimation(CreateSkill(LegacyDrawSkill(descriptor), descriptor.JingJie), preferredDeckIndex);

    public void LegacyAddSkillProcedure(SkillEntry skillEntry, JingJie? preferredJingJie = null, DeckIndex? preferredDeckIndex = null)
        => LegacyAddSkill(CreateSkill(skillEntry, preferredJingJie), preferredDeckIndex);

    public void AddSkillProcedure(SkillEntry skillEntry, JingJie? preferredJingJie = null, DeckIndex? preferredDeckIndex = null)
        => AddSkillProcedure(CreateSkill(skillEntry, preferredJingJie), preferredDeckIndex);

    public void AddSkillProcedure(RunSkill skill, DeckIndex? preferredDeckIndex = null)
    {
        if (!preferredDeckIndex.HasValue)
        {
            int last = Hand.Count();
            Hand.Add(skill);
            AddSkillNeuron.Invoke(new AddSkillDetails(DeckIndex.FromHand(last), skill));
            return;
        }
        
        DeckIndex deckIndex = preferredDeckIndex.Value;
        if (deckIndex.InField)
        {
            Home.GetSlot(deckIndex.Index).Skill = skill;
            AddSkillNeuron.Invoke(new AddSkillDetails(deckIndex, skill));
            return;
        }
        
        Hand.Replace(deckIndex.Index, skill);
        AddSkillNeuron.Invoke(new AddSkillDetails(deckIndex, skill));
    }
    
    public void DrawSkillProcedure(SkillEntryDescriptor descriptor, DeckIndex? preferredDeckIndex = null)
    {
        SkillPool.Shuffle();
        SkillPool.TryPopItem(out SkillEntry skillEntry, descriptor.Contains);
        skillEntry ??= Encyclopedia.SkillCategory[0];
        
        AddSkillProcedure(skillEntry,
            preferredJingJie: descriptor.JingJie,
            preferredDeckIndex: preferredDeckIndex);
    }

    public void PickDiscoveredSkillProcedure(int pickedIndex, SkillEntryDescriptor skillDescriptor)
    {
        RunSkill skill = CreateSkill(skillDescriptor.Entry, skillDescriptor.JingJie);
        int last = Hand.Count();
        Hand.Add(skill);
        PickDiscoveredSkillNeuron.Invoke(new(DeckIndex.FromHand(last), pickedIndex));
    }

    public void BuySkillProcedure(Commodity commodity, int commodityIndex)
    {
        RunSkill skill = CreateSkill(commodity.Skill.Entry, commodity.Skill.JingJie);
        int last = Hand.Count();
        Hand.Add(skill);
        BuySkillNeuron.Invoke(new(DeckIndex.FromHand(last), commodityIndex));
    }

    public void GachaProcedure(SkillEntryDescriptor skillDescriptor, int gachaIndex)
    {
        RunSkill skill = CreateSkill(skillDescriptor.Entry, skillDescriptor.JingJie);
        int last = Hand.Count();
        Hand.Add(skill);
        GachaNeuron.Invoke(new(DeckIndex.FromHand(last), gachaIndex));
    }

    #endregion
}
