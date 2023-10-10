
using System;
using System.Collections.Generic;
using CLLibrary;

public class EditorManager : Singleton<EditorManager>, Addressable
{
    public event Action EnvironmentChangedEvent;
    public void EnvironmentChanged() => EnvironmentChangedEvent?.Invoke();

    [NonSerialized] public EntityEditableList EntityEditableList;
    [NonSerialized] public int? _selectionIndex;

    [NonSerialized] public RunEntity Home;

    [NonSerialized] public StageEnvironmentResult SimulateResult;

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

        EntityEditableList = EntityEditableList.ReadFromFile();

        Home = RunEntity.Default();
        Home.EnvironmentChangedEvent += EnvironmentChanged;
        EnvironmentChangedEvent += Simulate;
    }

    private RunEntity GetAway()
    {
        if (_selectionIndex == null)
            return RunEntity.FromJingJieHealth(Home.GetJingJie(), 1000000);

        return EntityEditableList[_selectionIndex.Value];
    }

    public void Combat()
    {
        RunEntity away = GetAway();
        StageConfig d = new StageConfig(true, false, false, false, Home, away);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
    }

    public void Simulate()
    {
        RunEntity away = GetAway();
        StageConfig d = new StageConfig(false, false, false, false, Home, away);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
        SimulateResult = environment.Result;
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
        EntityEditableList.WriteToFile();
    }

    public void Load()
    {
        EntityEditableList = EntityEditableList.ReadFromFile();
    }

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
