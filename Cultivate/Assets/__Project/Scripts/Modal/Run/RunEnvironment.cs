
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
        InitSkillPool();

        Map.Init();
        
        SetJingJieProcedure(Map.Entry._envJingJie);
        _home.SetSlotCount(Map.Entry._slotCount);
        SetDGoldProcedure(Map.Entry._gold);
        DrawSkillsProcedure(new(jingJie: Map.Entry._skillJingJie, count: Map.Entry._skillCount));
        
        _eventDict.SendEvent(RunEventDict.START_RUN, d);
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
        _eventDict.SendEvent(RunEventDict.WIL_SET_JINGJIE, d);
        if (d.Cancel)
            return;

        _jingJie = d.ToJingJie;
        
        // move to ascension procedure
        _home.SetBaseHealth(RunEntity.BaseHealthFromJingJie[d.ToJingJie]);
        
        _home.SetJingJie(d.ToJingJie);
        AudioManager.Play(Encyclopedia.AudioFromJingJie(d.ToJingJie));

        _eventDict.SendEvent(RunEventDict.DID_SET_JINGJIE, d);
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
            return panelDescriptor;
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
    private JingJie _jingJie;
    public JingJie JingJie => _jingJie;
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

        Map = new(_config.MapEntry);
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
        => _runResultPanelDescriptor ?? Map.Panel;

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
