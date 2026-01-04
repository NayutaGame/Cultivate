
using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;
using UnityEngine;

[Serializable]
public class Map : Addressable, ISerializationCallbackReceiver
{
    public static int[,] StepCount = new int[6, 2]
    {
        { 5, 3 },
        { 7, 4 },
        { 9, 5 },
        { 11, 6 },
        { 13, 7 },
        { 13, 7 },
    };

    public static int GetTotalStepCountFromJingJie(JingJie jingJie)
        => StepCount[jingJie.GetIndex(), 0];

    public static int GetAvailableStepCountFromJingJie(JingJie jingJie)
        => StepCount[jingJie.GetIndex(), 1];
    
    [SerializeField] private MapEntry _entry;
    
    [SerializeReference] private Level[] _levels;
    [SerializeField] private int _levelIndex;
    [SerializeField] private int _stepIndex;

    #region Core
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "CurrLevel",                  thisObject => ((Map)thisObject).GetCurrLevel() },
    };
    public object Get(string s) => Accessor[s](this);
    public Map(MapEntry entry)
    {
        _entry = entry;
    }
    
    public MapEntry GetEntry() => _entry;

    public void Init(Profile profile, RunEnvironment env)
    {
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

    public ICellAdapter CreateCellFromCurrRoom()
    {
        ICellAdapter cell = GetCurrRoom().CreatePanel(this);
        GetCurrRoom().SetState(Room.RoomState.Curr);
        return cell;
    }

    #endregion

    #region Accessors
    
    public Level GetCurrLevel() => _levels[_levelIndex];
    public Room GetCurrRoom() => GetCurrLevel().GetRoom(_stepIndex);

    public Sprite GetCurrEventIllustration()
    {
        return Encyclopedia.SpriteCategory.MissingEventIllustration().Sprite;
        // Room room = GetCurrRoom();
        // return room?.GetEntry().GetSprite() ?? Encyclopedia.SpriteCategory.MissingEventIllustration().Sprite;
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
