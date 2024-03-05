
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
        EnvironmentChanged();
    }

    [NonSerialized] public RunEntity Home;

    [NonSerialized] public StageResult SimulateResult;

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

        Load();

        Home = RunEntity.Default();
        Home.EnvironmentChangedEvent += EnvironmentChanged;
        EnvironmentChangedEvent += SimulateProcedure;
    }

    private RunEntity GetAway()
    {
        if (_selectionIndex == null)
            return RunEntity.FromJingJieHealth(Home.GetJingJie(), 1000000);

        return EntityEditableList[_selectionIndex.Value];
    }

    public void Combat()
    {
        StageEnvironment.Combat(StageConfig.ForEditor(Home, GetAway(), null));
    }

    public void SimulateProcedure()
    {
        PlacementProcedure();
        FormationProcedure();
        
        SimulateResult = StageEnvironment.CalcSimulateResult(StageConfig.ForSimulate(Home, GetAway(), null));
    }

    private void PlacementProcedure()
    {
        Home.PlacementProcedure();
        GetAway().PlacementProcedure();
    }

    private void FormationProcedure()
    {
        Home.FormationProcedure();
        GetAway().FormationProcedure();
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
