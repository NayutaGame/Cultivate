
using System;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

[Serializable]
public class Level : Addressable, ISerializationCallbackReceiver
{
    [SerializeReference] private RoomListModel _rooms;
    
    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public Level(RoomDescriptor[] stepDescriptors)
    {
        _accessors = new()
        {
            { "Rooms", () => _rooms },
        };
        
        _rooms = new();
        for (int i = 0; i < stepDescriptors.Length; i++)
        {
            _rooms.Add(new Room(stepDescriptors[i]));
        }
    }
    
    public Room GetRoom(int stepIndex) => _rooms[stepIndex];
    public int GetRoomCount() => _rooms.Count();
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _accessors = new()
        {
            { "Rooms", () => _rooms },
        };
    }
}
