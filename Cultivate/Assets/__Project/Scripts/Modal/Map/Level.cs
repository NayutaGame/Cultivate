
using System;
using System.Collections.Generic;

public class Level : Addressable
{
    private RoomListModel _rooms;

    public Room GetRoom(int stepIndex)
        => _rooms[stepIndex];
    public int GetRoomCount()
        => _rooms.Count();
    
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
}
