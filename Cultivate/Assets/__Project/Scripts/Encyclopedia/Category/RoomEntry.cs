
using System;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[Serializable]
public class RoomEntry : Entry
{
    [NonSerialized] private string _roomPath;
    [NonSerialized] private LogicGraph _roomGraph;
    
    public RoomEntry(
        string id,
        string name,
        string roomPath,
        LogicGraph roomGraph
        ) : base(id, name)
    {
        _roomPath = roomPath;
        _roomGraph = roomGraph;
    }

    public LogicGraph RoomGraph => _roomGraph;
}
