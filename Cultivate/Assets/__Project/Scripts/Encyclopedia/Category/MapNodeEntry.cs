
using PuppyDragon.uNody.Logic;
using UnityEngine;

public class MapNodeEntry : Entry
{
    private string _logicGraphPath;
    private LogicGraph _roomGraph;
    
    public MapNodeEntry(string id, string name) : base(id, name)
    {
        _logicGraphPath = $"MapNodeLogicGraph/{GetName()}";
        _roomGraph = Resources.Load<LogicGraph>(_logicGraphPath);
    }

    public LogicGraph RoomGraph => _roomGraph;
}
