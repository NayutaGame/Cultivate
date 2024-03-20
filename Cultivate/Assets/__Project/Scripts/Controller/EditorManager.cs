
using System;
using System.Collections.Generic;
using CLLibrary;

public class EditorManager : Singleton<EditorManager>, Addressable
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    [NonSerialized] public EntityEditableList EntityEditableList;
    [NonSerialized] private int? _selectionIndex;
    public int? GetSelectionIndex() => _selectionIndex;
    public void SetSelectionIndex(int? value)
    {
        _selectionIndex = value;
        
        SetAwayFromSelectionIndex(value);
        
        EnvironmentChanged();
    }

    [NonSerialized] private RunEntity _home;
    [NonSerialized] private RunEntity _away;

    private void SetAway(RunEntity away)
    {
        if (_away != null)
            _away.EnvironmentChangedEvent -= EnvironmentChanged;
        
        _away = away;
        
        if (_away != null)
            _away.EnvironmentChangedEvent += EnvironmentChanged;
    }

    private void SetAwayFromSelectionIndex(int? selectionIndex)
    {
        SetAway(_selectionIndex.HasValue
            ? EntityEditableList[_selectionIndex.Value]
            : RunEntity.FromJingJieHealth(_home.GetJingJie(), 1000000));
    }

    [NonSerialized] public StageResult SimulateResult;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public override void DidAwake()
    {
        base.DidAwake();

        _accessors = new()
        {
            { "EntityEditableList", () => EntityEditableList },
            { "Home", () => _home }
        };

        Load();

        _home = RunEntity.Default();
        _home.EnvironmentChangedEvent += EnvironmentChanged;
        EnvironmentChangedEvent += SimulateProcedure;
    }

    public void Combat()
    {
        StageEnvironment.Combat(StageConfig.ForEditor(_home, _away, null));
    }

    public void SimulateProcedure()
    {
        PlacementProcedure();
        FormationProcedure();
        
        SimulateResult = StageEnvironment.CalcSimulateResult(StageConfig.ForSimulate(_home, _away, null));
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

    public void CopyToTop()
    {
        if (_selectionIndex == null)
            return;
        EntityEditableList.Replace(EntityEditableList[_selectionIndex.Value], RunEntity.FromTemplate(_home));
    }

    public void SwapTopAndBottom()
    {
        if (_selectionIndex == null)
            return;
        RunEntity temp = _home;
        _home = EntityEditableList[_selectionIndex.Value];
        EntityEditableList.Replace(EntityEditableList[_selectionIndex.Value], temp);
    }

    public void CopyToBottom()
    {
        if (_selectionIndex == null)
            return;
        _home = RunEntity.FromTemplate(EntityEditableList[_selectionIndex.Value]);
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
