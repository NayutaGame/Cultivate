
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Level : Addressable
{
    [SerializeReference] private RoomListModel _rooms;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Rooms",                      thisObject => ((Level)thisObject)._rooms },
    };
    public object Get(string s) => Accessor[s](this);
    public Level(RoomDefinition[] stepDescriptors)
    {
        _rooms = new();
        for (int i = 0; i < stepDescriptors.Length; i++)
        {
            _rooms.Add(new Room(stepDescriptors[i]));
        }
    }
    
    public Room GetRoom(int stepIndex) => _rooms[stepIndex];
    public int GetRoomCount() => _rooms.Count();
}
