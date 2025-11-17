
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
        string name
        ) : base(id, name)
    {
        _roomPath = $"RoomGraphs/{GetName()}";
        _roomGraph = Resources.Load<LogicGraph>(_roomPath);
    }

    public LogicGraph RoomGraph => _roomGraph;
}
