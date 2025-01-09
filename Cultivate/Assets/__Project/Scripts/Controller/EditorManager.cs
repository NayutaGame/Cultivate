
using System;
using System.Collections.Generic;
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
            _home?.EnvironmentChangedNeuron.Remove(EnvironmentChangedNeuron);
            _home = value;
            _home?.EnvironmentChangedNeuron.Add(EnvironmentChangedNeuron);
        }
    }

    [NonSerialized] private RunEntity _away;
    private RunEntity Away
    {
        get => _away;
        set
        {
            _away?.EnvironmentChangedNeuron.Remove(EnvironmentChangedNeuron);
            _away = value;
            _away?.EnvironmentChangedNeuron.Add(EnvironmentChangedNeuron);
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

    [NonSerialized] public StageResult SimulateResult;

    [NonSerialized] private RunConfig _config;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "EntityEditableList", () => EntityEditableList },
            { "Home", () => Home }
        };
        EnvironmentChangedNeuron = new();

        Load();
        
        Home = RunEntity.Default();
        Away = RunEntity.Trainer();
        EnvironmentChangedNeuron.Add(SimulateProcedure);
    }

    public void Combat()
    {
        StageEnvironment.Combat(StageConfig.ForEditor(Home, Away, _config));
    }

    private void SimulateProcedure()
    {
        if (RunManager.Instance.Environment == null)
        {
            _config = RunConfig.FirstRun();
            RunManager.Instance.SetEnvironmentFromConfig(_config);
        }
        
        PlacementProcedure();
        FormationProcedure();
        
        SimulateResult = StageEnvironment.CalcSimulateResult(StageConfig.ForSimulate(Home, Away, _config));
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

    public void CopyToTop()
    {
        if (_selectionIndex == null)
            return;
        EntityEditableList.Replace(EntityEditableList[_selectionIndex.Value], RunEntity.FromTemplate(Home));
    }

    public void SwapTopAndBottom()
    {
        if (_selectionIndex == null)
            return;
        RunEntity temp = Home;
        Home = EntityEditableList[_selectionIndex.Value];
        EntityEditableList.Replace(EntityEditableList[_selectionIndex.Value], temp);
    }

    public void CopyToBottom()
    {
        if (_selectionIndex == null)
            return;
        Home = RunEntity.FromTemplate(EntityEditableList[_selectionIndex.Value]);
    }

    public void Save()
    {
        FileUtility.WriteToFile(EntityEditableList, EntityEditableList.Filename);
    }

    public void Load()
    {
        EntityEditableList = FileUtility.ReadFromFile<EntityEditableList>(EntityEditableList.Filename);
    }

    public static RunEntity FindEntity(string name)
        => Instance.EntityEditableList.Traversal().FirstObj(e => e.GetEntry().GetName() == name);

    // public bool TrySwap(SkillSlot fromSlot, SkillSlot toSlot)
    // {
    //     EmulatedSkill temp = fromSlot.Skill;
    //     fromSlot.Skill = toSlot.Skill;
    //     toSlot.Skill = temp;
    //     return true;
    // }
    //
    // public bool TryWrite(RunSkill fromSkill, SkillSlot toSlot)
    // {
    //     toSlot.Skill = fromSkill;
    //     return true;
    // }
    //
    // public bool TryWrite(SkillSlot fromSlot, SkillSlot toSlot)
    // {
    //     toSlot.Skill = fromSlot.Skill;
    //     return true;
    // }
    //
    // public bool TryIncreaseJingJie(RunSkill skill)
    // {
    //     bool success = skill.TryIncreaseJingJie();
    //     if (!success)
    //         return false;
    //     EnvironmentChanged();
    //     return false;
    // }
    //
    // public bool TryIncreaseJingJie(SkillSlot slot)
    // {
    //     bool success = slot.TryIncreaseJingJie();
    //     if (!success)
    //         return false;
    //     EnvironmentChanged();
    //     return false;
    // }
}
