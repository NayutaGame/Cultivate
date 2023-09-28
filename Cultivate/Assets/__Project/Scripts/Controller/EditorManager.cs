
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

        Home = RunEntity.Default;
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
        StageEnvironmentDetails d = new StageEnvironmentDetails(true, false, false, false, Home, away);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
    }

    public void Simulate()
    {
        RunEntity away = GetAway();
        StageEnvironmentDetails d = new StageEnvironmentDetails(false, false, false, false, Home, away);
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
}
