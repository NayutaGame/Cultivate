
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
    
    public MapEntry GetEntry() => _entry;

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
    }

    private void InitEntityPool()
    {
        EntityPool = new();
        int difficulty = RunManager.Instance.Environment.GetRunConfig().DifficultyProfile.GetEntry()._order;
        EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.Traversal().FilterObj(
            e => e.IsInPool() && e.GetAllowedDifficulty().Contains(difficulty)));
        EntityPool.Shuffle();
    }

    private void InitAdventurePool()
    {
        RoomPool = new();
        RoomPool.Populate(Encyclopedia.RoomCategory.Traversal.FilterObj(e => e.WithInPool));
        RoomPool.Shuffle();
    }

    public void Step()
    {
        if (IsAboutToFinish())
            throw new Exception("Shouldn't be here");

        if (IsLastStep())
        {
            NextLevel();
            return;
        }

        NextStep();
    }

    public void NextLevel()
    {
        GetCurrRoom().SetState(Room.RoomState.Past);
        
        _levelIndex++;
        _stepIndex = 0;
    }

    public void NextStep()
    {
        GetCurrRoom().SetState(Room.RoomState.Past);
        
        _stepIndex++;
    }

    public void InsertRoom(RoomEntry roomEntry)
    {
        InsertedRoomPool.Populate(roomEntry);
        InsertedRoomPool.Shuffle();
    }

    public PanelDescriptor CreatePanelFromCurrRoom()
    {
        var panel = GetCurrRoom().CreatePanel(this);
        GetCurrRoom().SetState(Room.RoomState.Curr);
        return panel;
    }

    #endregion

    #region Accessors
    
    public Level GetCurrLevel() => _levels[_levelIndex];
    public Room GetCurrRoom() => GetCurrLevel().GetRoom(_stepIndex);

    public bool IsAboutToFinish()
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
