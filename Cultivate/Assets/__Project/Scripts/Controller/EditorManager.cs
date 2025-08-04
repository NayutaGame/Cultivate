
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

public class EditorManager : Singleton<EditorManager>, Addressable
{
    public Neuron EnvironmentChangedNeuron;

    [NonSerialized] public EntityEditableList EntityEditableList;
    
    [NonSerialized] private RunEntity _home;
    private RunEntity Home
    {
        get => _home;
        set
        {
            _home?.ChangedNeuron.Remove(EnvironmentChangedNeuron);
            _home = value;
            _home?.ChangedNeuron.Add(EnvironmentChangedNeuron);
        }
    }

    [NonSerialized] private RunEntity _away;
    private RunEntity Away
    {
        get => _away;
        set
        {
            _away?.ChangedNeuron.Remove(EnvironmentChangedNeuron);
            _away = value;
            _away?.ChangedNeuron.Add(EnvironmentChangedNeuron);
        }
    }
    
    [NonSerialized] private int? _selectionIndex;
    public int? GetSelectionIndex() => _selectionIndex;
    public void SetSelectionIndex(int? value)
    {
        _selectionIndex = value;
        SetAwayFromSelectionIndex(value);
        EnvironmentChangedNeuron.Invoke();
    }
    
    private void SetAwayFromSelectionIndex(int? selectionIndex)
    {
        Away = selectionIndex.HasValue
            ? EntityEditableList[selectionIndex.Value]
            : RunEntity.Trainer();
    }

    [NonSerialized] private string _skillSearchText;
    public string GetSkillSearchText() => _skillSearchText;
    public void SetSkillSearchText(string value)
    {
        _skillSearchText = value;
        FilteredSkillInventory.Refresh();
    }

    [NonSerialized] public FilteredListModel<RunSkill> FilteredSkillInventory;

    [NonSerialized] private Dirty<StageResult> _simulateResult;
    public StageResult GetSimulateResult() => _simulateResult.Value;

    [NonSerialized] private RunConfig _config;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "EntityEditableList",         thisObject => ((EditorManager)thisObject).EntityEditableList },
        { "Home",                       thisObject => ((EditorManager)thisObject).Home },
        { "FilteredSkillInventory",     thisObject => ((EditorManager)thisObject).FilteredSkillInventory },
    };
    public object Get(string s) => Accessor[s](this);
    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        EnvironmentChangedNeuron = new();

        Load();
        
        Home = RunEntity.Default();
        Away = RunEntity.Trainer();
        
        _simulateResult = new(Simulate);
        EnvironmentChangedNeuron.Add(_simulateResult.SetDirty);

        FilteredSkillInventory = new(AppManager.Instance.SkillInventory, s => s.GetEntry().MatchSearchText(GetSkillSearchText()));
        SetSkillSearchText("");
    }

    public void Combat()
    {
        AppManager.Instance.Push(AppStateMachine.STAGE, StageConfig.ForEditor(Home, Away, _config));
    }

    private StageResult Simulate()
    {
        if (RunManager.Instance.Environment == null)
        {
            _config = RunConfig.LastDifficulty();
            RunManager.Instance.SetEnvironmentFromConfig(_config);
        }
        
        PlacementProcedure();
        FormationProcedure();
        SecondPlacementProcedure();
        
        return StageResult.FromConfig(StageConfig.ForSimulate(Home, Away, _config));
    }

    private void PlacementProcedure()
    {
        Home.PlacementProcedure();
        Away.PlacementProcedure();
    }

    private void FormationProcedure()
    {
        Home.FormationProcedure();
        Away.FormationProcedure();
    }

    private void SecondPlacementProcedure()
    {
        Home.SecondPlacementProcedure();
        Away.SecondPlacementProcedure();
    }

    public void CopyToTop()
    {
        if (_selectionIndex == null)
            return;
        EntityEditableList.Replace(EntityEditableList[_selectionIndex.Value], RunEntity.FromTemplate(Home));
        EnvironmentChangedNeuron.Invoke();
    }

    public void SwapTopAndBottom()
    {
        if (_selectionIndex == null)
            return;
        RunEntity temp = Home;
        Home = EntityEditableList[_selectionIndex.Value];
        EntityEditableList.Replace(EntityEditableList[_selectionIndex.Value], temp);
        EnvironmentChangedNeuron.Invoke();
    }

    public void CopyToBottom()
    {
        if (_selectionIndex == null)
            return;
        Home = RunEntity.FromTemplate(EntityEditableList[_selectionIndex.Value]);
        EnvironmentChangedNeuron.Invoke();
    }

    public void Save()
    {
        FileUtility.WriteStreamingFile(EntityEditableList, EntityEditableList.Filename);
    }

    private void LoadOrDefault()
    {
        if (!FileUtility.IsPersistentFileExists(ProfileList.Filename))
        {
            NewProfile();
        }
        else
        {
            Load();
        }
    }

    public void NewProfile()
    {
        EntityEditableList = new EntityEditableList();
        Save();
    }

    public void Load()
    {
        try
        {
            EntityEditableList = FileUtility.ReadStreamingFile<EntityEditableList>(EntityEditableList.Filename);
        }
        catch
        {
            Debug.Log("检测到存档过时或者损坏，已经创建新存档");
            EntityEditableList = null;
        }

        if (EntityEditableList == null) ;
        {
            NewProfile();
        }
    }

    public static RunEntity FindEntity(string name)
        => Instance.EntityEditableList.FirstObj(e => e.GetEntry().GetName() == name);

    public void TryWrite(RunSkill skill, SkillSlot slot)
    {
        bool skillInventoryContainsSkill = FilteredSkillInventory.Contains(skill);
        bool homeOrAwayContainsSlot = Home.TraversalCurrentSlots().Any(s => s == slot) || 
                                      Away.TraversalCurrentSlots().Any(s => s == slot);
        if (!skillInventoryContainsSkill || !homeOrAwayContainsSlot)
            return;

        slot.Skill = skill;
        EnvironmentChangedNeuron.Invoke();
    }

    public void TrySwap(SkillSlot fromSlot, SkillSlot toSlot)
    {
        bool homeOrAwayContainsFrom = Home.TraversalCurrentSlots().Any(s => s == fromSlot) || 
                                      Away.TraversalCurrentSlots().Any(s => s == fromSlot);
        bool homeOrAwayContainsTo = Home.TraversalCurrentSlots().Any(s => s == toSlot) || 
                                    Away.TraversalCurrentSlots().Any(s => s == toSlot);
        bool fromEqualsTo = fromSlot == toSlot;
        bool fromHasSkill = fromSlot.Skill != null;
        if (!homeOrAwayContainsFrom || !homeOrAwayContainsTo || fromEqualsTo || !fromHasSkill)
            return;
        
        RunSkill temp = fromSlot.Skill;
        fromSlot.Skill = toSlot.Skill;
        toSlot.Skill = temp;
        EnvironmentChangedNeuron.Invoke();
    }

    public void TryClear(SkillSlot slot)
    {
        bool homeOrAwayContainsSlot = Home.TraversalCurrentSlots().Any(s => s == slot) || 
                                      Away.TraversalCurrentSlots().Any(s => s == slot);
        bool slotHasSkill = slot.Skill != null;
        if (!homeOrAwayContainsSlot || !slotHasSkill)
            return;
        
        slot.Skill = null;
        EnvironmentChangedNeuron.Invoke();
    }

    public void TryIncreaseJingJie(SkillSlot slot)
    {
        bool homeOrAwayContainsSlot = Home.TraversalCurrentSlots().Any(s => s == slot) || 
                                      Away.TraversalCurrentSlots().Any(s => s == slot);
        bool slotHasSkill = slot.Skill != null;
        if (!homeOrAwayContainsSlot || !slotHasSkill)
            return;

        SkillEntry skillEntry = slot.Skill.GetEntry();
        JingJie currJingJie = slot.Skill.JingJie;
        JingJie nextJingJie = skillEntry.JingJieContains(currJingJie + 1)
            ? currJingJie + 1
            : skillEntry.GetLowestJingJie();
        slot.Skill.JingJie = nextJingJie;
        EnvironmentChangedNeuron.Invoke();
    }

    public void InsertAt(int index, RunEntity template = null)
    {
        RunEntity toInsert = template ?? RunEntity.FromTemplate(EntityEditableList[index]);
        EntityEditableList.Insert(index, toInsert);
    }

    public void Add(RunEntity template = null)
    {
        RunEntity toInsert = template ?? RunEntity.Default();
        EntityEditableList.Add(toInsert);
    }
}
