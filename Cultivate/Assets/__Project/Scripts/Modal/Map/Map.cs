
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
    public Pool<RoomEntry> RoomPool;
    public Pool<RoomEntry> InsertedRoomPool;
    
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
    public Room CurrRoom => CurrLevel.GetRoom(_stepIndex);

    public void Init()
    {
        InitEntityPool();
        InitAdventurePool();
        InsertedRoomPool = new();

        _levels = new Level[_entry.Levels.Length];
        for (int i = 0; i < _entry.Levels.Length; i++)
            _levels[i] = new Level(_entry.Levels[i]);

        _stepIndex = 0;
        _levelIndex = 0;
        
        CreatePanel();
    }

    public void NextLevel()
    {
        _levelIndex++;
        _stepIndex = 0;
        
        CreatePanel();
    }

    public void NextStep()
    {
        _stepIndex++;
        
        CreatePanel();
    }

    public bool IsLastLevelAndLastStep()
        => _levels.Length - 1 == _levelIndex && IsLastStep();

    public bool IsLastStep()
        => CurrLevel.GetRoomCount() - 1 == _stepIndex;

    public void CreatePanel()
    {
        if (!CurrRoom.HasEntry())
            CurrRoom.DrawEntry(this);
        CurrRoom.CreatePanel(this);
    }

    public void InsertRoom(RoomEntry roomEntry)
    {
        InsertedRoomPool.Populate(roomEntry);
        InsertedRoomPool.Shuffle();
    }

    private void InitEntityPool()
    {
        EntityPool = new();
        EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal().FilterObj(e => e.IsInPool()));
        EntityPool.Shuffle();
    }

    private void InitAdventurePool()
    {
        RoomPool = new();
        RoomPool.Populate(Encyclopedia.RoomCategory.Traversal.FilterObj(e => e.WithInPool));
        RoomPool.Shuffle();
    }
}
