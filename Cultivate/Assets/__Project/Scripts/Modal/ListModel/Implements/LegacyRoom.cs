
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class LegacyRoom : ISerializationCallbackReceiver, AnnotatableRoom
{
    public enum RoomState
    {
        Past,
        Curr,
        Future,
    }

    [SerializeField] private RoomState _state;
    [SerializeReference] private RoomDefinition _roomDefinition;
    [SerializeField] private LegacyRoomEntry _entry;
    [SerializeReference] private RunEntity _predrewRunEntity;


    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public LegacyRoom(RoomDefinition roomDefinition)
    {
        _roomDefinition = roomDefinition;
        _state = RoomState.Future;
    }
    
    public RoomState GetState() => _state;
    public void SetState(RoomState state) => _state = state;
    public RoomDefinition GetDescriptor() => _roomDefinition;
    public LegacyRoomEntry GetEntry() => _entry;
    public RunEntity GetPredrewRunEntity() => _predrewRunEntity;
    public void SetPredrewRunEntity(RunEntity runEntity) => _predrewRunEntity = runEntity;

    public int Ladder => _roomDefinition.Ladder;

    public Cell CreatePanel(Map map)
    {
        _entry ??= _roomDefinition.Draw(map, this);
        return _entry.Create(map, this);
    }
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetId()) ? null : Encyclopedia.LegacyRoomCategory.FromId(_entry.GetId());
    }

    public bool CanShowAnnotation()
        => true;

    public string GetTitle()
        => GetDescriptor().GetTitle();

    public Description GetDescription()
        => GetDescriptor().GetDescription();
}
