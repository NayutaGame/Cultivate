
using System;
using System.Collections.Generic;
using CLLibrary;

public class Map : Addressable
{
    private MapEntry _entry;
    public MapEntry Entry => _entry;
    
    private Level[] _levels;
    private int _levelIndex;
    private int _stepIndex;

    private PanelDescriptor _panel;
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
    
    public EntityPool EntityPool;
    public Pool<NodeEntry> AdventurePool;
    public Pool<NodeEntry> InsertedAdventurePool;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Map(MapEntry entry)
    {
        _accessors = new()
        {
            { "CurrLevel", () => CurrLevel },
        };

        _entry = entry;
    }

    public Level CurrLevel => _levels[_levelIndex];
    public StepItem CurrStepItem => CurrLevel.GetStepItem(_stepIndex);
    public RunNode CurrNode => CurrStepItem.Node;
    public StepDescriptor CurrStepDescriptor => _entry.GetStepDescriptorFromLevelAndStep(_levelIndex, _stepIndex);

    public void Init()
    {
        InitEntityPool();
        InitAdventurePool();
        InsertedAdventurePool = new();

        _levels = new Level[_entry.Levels.Length];
        for (int i = 0; i < _entry.Levels.Length; i++)
            _levels[i] = new Level(_entry.Levels[i]);
    }

    public void NextLevel()
    {
        _levelIndex++;
        _stepIndex = 0;
    }

    public void NextStep()
    {
        _stepIndex++;
        if (!CurrStepItem.HasNode())
            CurrStepItem.DrawNode(this);
    }

    public bool IsLastLevelAndLastStep()
        => _levels.Length - 1 == _levelIndex && IsLastStep();

    public bool IsLastStep()
        => CurrLevel.GetStepCount() - 1 == _stepIndex;

    public void CreatePanel()
        => CurrStepItem.CreatePanel(this);

    public void InsertAdventure(NodeEntry nodeEntry)
    {
        InsertedAdventurePool.Populate(nodeEntry);
        InsertedAdventurePool.Shuffle();
    }

    private void InitEntityPool()
    {
        EntityPool = new();
        EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal().FilterObj(e => e.IsInPool()));
        EntityPool.Shuffle();
    }

    private void InitAdventurePool()
    {
        AdventurePool = new();
        AdventurePool.Populate(Encyclopedia.NodeCategory.Traversal.FilterObj(e => e.WithInPool));
        AdventurePool.Shuffle();
    }
}
