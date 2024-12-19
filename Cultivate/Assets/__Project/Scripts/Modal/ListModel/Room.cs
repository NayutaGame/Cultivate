
using System;
using UnityEngine;

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
    [SerializeReference] private RoomDescriptor _descriptor;
    [SerializeField] private RoomEntry _entry;
    [SerializeReference] private RunEntity _predrewRunEntity;

    public Room(RoomDescriptor descriptor)
    {
        _descriptor = descriptor;
        _state = RoomState.Future;
    }
    
    public RoomState GetState() => _state;
    public void SetState(RoomState state) => _state = state;
    public RoomDescriptor GetDescriptor() => _descriptor;
    public RunEntity GetPredrewRunEntity() => _predrewRunEntity;
    public void SetPredrewRunEntity(RunEntity runEntity) => _predrewRunEntity = runEntity;

    public int Ladder => _descriptor.Ladder;

    public PanelDescriptor CreatePanel(Map map)
    {
        _entry ??= _descriptor.Draw(map, this);
        return _entry.Create(map, this);
    }
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _entry = string.IsNullOrEmpty(_entry.GetName()) ? null : Encyclopedia.RoomCategory[_entry.GetName()];
    }
}
