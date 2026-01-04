
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using Unity.VisualScripting;

public class RoomEnvironment
{
    private MapNode _mapNode;
    private RoomEntry _roomEntry;
    private int _ladder;

    private Memory _memory;
    private LogicGraph _roomGraph;
    private int _index;

    public CellNode CurrentCell => _roomGraph.CurrentNode as CellNode;
    
    private RoomEnvironment(MapNode mapNode, RoomEntry roomEntry, int ladder)
    {
        _mapNode = mapNode;
        _roomEntry = roomEntry;
        _ladder = ladder;
        
        _memory = new Memory();

        ResetRoomGraph();
    }

    private void ResetRoomGraph()
    {
        _roomGraph = _roomEntry.RoomGraph;
        _roomGraph.Reset();
        foreach (Node node in _roomGraph.Nodes)
            if (node is CellNode cellNode)
                cellNode.Reset();
        _roomGraph.Blackboard.SetLocalValue(_roomGraph, "Ladder", _ladder);
    }

    public static RoomEnvironment CreateRoom(MapNode mapNode, RoomEntry roomEntry, int ladder)
    {
        return new RoomEnvironment(mapNode, roomEntry, ladder);
    }

    public static RoomEnvironment CreateRoom(RoomEntry roomEntry, int ladder)
    {
        return new RoomEnvironment(null, roomEntry, ladder);
    }

    public void Step()
    {
        _roomGraph.Step();
    }

    public int GetLadder()
        => _ladder;

    public void ReceiveSignal(Signal signal)
    {
        bool cellIsEnded = CurrentCell.ReceiveSignal(signal);
        if (cellIsEnded)
            Step();
    }

    public bool IsFinished()
        => CurrentCell == null;
}