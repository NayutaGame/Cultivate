
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

[Serializable]
public class Map : Addressable, ISerializationCallbackReceiver
{
    [SerializeField] private MapEntry _entry;
    
    [SerializeReference] private Level[] _levels;
    [SerializeField] private int _levelIndex;
    [SerializeField] private int _stepIndex;
    [SerializeReference] public EntityPool EntityPool;
    [SerializeReference] public RoomPool RoomPool;
    [SerializeReference] public RoomPool InsertedRoomPool;
    [NonSerialized] private PanelDescriptor _panel;

    #region Core
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Map(MapEntry entry)
    {
        _accessors = new()
        {
            { "CurrLevel", GetCurrLevel },
        };

        _entry = entry;
    }

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
        
        SetPanelFromCurrRoom();
        GetCurrRoom().SetState(Room.RoomState.Curr);
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

    public void NextLevel()
    {
        GetCurrRoom().SetState(Room.RoomState.Past);
        
        _levelIndex++;
        _stepIndex = 0;
        
        SetPanelFromCurrRoom();
        GetCurrRoom().SetState(Room.RoomState.Curr);
    }

    public void NextStep()
    {
        GetCurrRoom().SetState(Room.RoomState.Past);
        
        _stepIndex++;

        SetPanelFromCurrRoom();
        GetCurrRoom().SetState(Room.RoomState.Curr);
    }

    public void InsertRoom(RoomEntry roomEntry)
    {
        InsertedRoomPool.Populate(roomEntry);
        InsertedRoomPool.Shuffle();
    }

    #endregion

    #region Accessors
    
    public MapEntry GetEntry() => _entry;
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

    public void SetPanelFromCurrRoom()
    {
        Panel = GetCurrRoom().CreatePanel(this);
    }
    
    public Level GetCurrLevel() => _levels[_levelIndex];
    public Room GetCurrRoom() => GetCurrLevel().GetRoom(_stepIndex);

    public bool IsLastLevelAndLastStep()
        => _levels.Length - 1 == _levelIndex && IsLastStep();

    public bool IsLastStep()
        => GetCurrLevel().GetRoomCount() - 1 == _stepIndex;

    #endregion
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "CurrLevel", GetCurrLevel },
        };
        
        _entry = string.IsNullOrEmpty(_entry.GetName()) ? null : Encyclopedia.MapCategory[_entry.GetName()];
    }
}
