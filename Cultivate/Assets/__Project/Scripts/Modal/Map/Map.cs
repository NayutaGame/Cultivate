
using System;
using System.Collections.Generic;
using System.Linq;
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

    // [SerializeReference]
    private MapNodeListModel _mapNodes;

    #region Core
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "CurrLevel",                  thisObject => ((Map)thisObject).GetCurrLevel() },
        { "MapNodes",                   thisObject => ((Map)thisObject)._mapNodes },
    };
    public object Get(string s) => Accessor[s](this);
    public Map(MapEntry entry)
    {
        _entry = entry;
        _mapNodes = new();
    }
    
    public MapEntry GetEntry() => _entry;

    public void Init(Profile profile, RunEnvironment env)
    {
        InitEntityPool();
        InitAdventurePool(env);
        InsertedRoomPool = new();

        CompileLevels(_entry.Levels, profile, env);

        _stepIndex = 0;
        _levelIndex = 0;
    }
    
    private void CompileLevels(RoomDefinition[][] levels, Profile profile, RunEnvironment env)
    {
        _levels = levels
            .Select(levelRooms =>
                levelRooms.Where(room => room != null && (room.Pred == null || room.Pred(profile, env)))
                        .ToArray()
            )
            .Where(compiledLevel => compiledLevel.Length > 0) // 筛掉空层
            .Select(compiledLevel => new Level(compiledLevel))
            .ToArray();
    }

    private void InitEntityPool()
    {
        EntityPool = new();
        int difficulty = RunManager.Instance.Environment.GetRunConfig().GetDifficulty();
        EntityPool.Populate(AppManager.Instance.EditorManager.EntityEditableList.FilterObj(
            e => e.IsInPool() && e.GetAllowedDifficulty().Contains(difficulty)));
        EntityPool.Shuffle();
    }

    private void InitAdventurePool(RunEnvironment env)
    {
        RoomPool = new();
        int difficulty = env.GetRunConfig().GetDifficulty();
        RoomPool.Populate(Encyclopedia.RoomCategory.FilterObj(e => e.WithInPool && e.DifficultyBound.Contains(difficulty)));
        RoomPool.Shuffle();
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

    public void InsertRoom(string roomName)
        => InsertRoom(Encyclopedia.RoomCategory.FromName(roomName));
    public void InsertRoom(RoomEntry roomEntry)
    {
        InsertedRoomPool.Populate(roomEntry);
        InsertedRoomPool.Shuffle();
    }

    public Cell CreatePanelFromCurrRoom()
    {
        var panel = GetCurrRoom().CreatePanel(this);
        GetCurrRoom().SetState(Room.RoomState.Curr);
        return panel;
    }

    #endregion

    #region Accessors
    
    public Level GetCurrLevel() => _levels[_levelIndex];
    public Room GetCurrRoom() => GetCurrLevel().GetRoom(_stepIndex);

    public Sprite GetCurrEventIllustration()
    {
        Room room = GetCurrRoom();
        return room?.GetEntry().GetSprite() ?? Encyclopedia.SpriteCategory.MissingEventIllustration().Sprite;
    }

    public bool IsAboutToFinish()
        => _levels.Length - 1 == _levelIndex && IsLastStep();

    public bool IsLastStep()
        => GetCurrLevel().GetRoomCount() - 1 == _stepIndex;

    #endregion
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.MapCategory.FromId(_entry.GetId());
    }
}
