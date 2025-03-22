
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Room : ISerializationCallbackReceiver
{
    public enum RoomState
    {
        Past,
        Curr,
        Future,
    }

    [SerializeField] private RoomState _state;
    [SerializeReference] private RoomDefinition _roomDefinition;
    [SerializeField] private RoomEntry _entry;
    [SerializeReference] private RunEntity _predrewRunEntity;

    public Room(RoomDefinition roomDefinition)
    {
        _roomDefinition = roomDefinition;
        _state = RoomState.Future;
    }
    
    public RoomState GetState() => _state;
    public void SetState(RoomState state) => _state = state;
    public RoomDefinition GetDescriptor() => _roomDefinition;
    public RoomEntry GetEntry() => _entry;
    public RunEntity GetPredrewRunEntity() => _predrewRunEntity;
    public void SetPredrewRunEntity(RunEntity runEntity) => _predrewRunEntity = runEntity;

    public int Ladder => _roomDefinition.Ladder;

    public PanelDescriptor CreatePanel(Map map)
    {
        _entry ??= _roomDefinition.Draw(map, this);
        return _entry.Create(map, this);
    }
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetName()) ? null : Encyclopedia.RoomCategory[_entry.GetName()];
    }
}
