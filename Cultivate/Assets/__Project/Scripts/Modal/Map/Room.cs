
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;

public class Room
{
    private Location _location;
    private RoomEntry _roomEntry;
    private int _ladder;
    private bool _isOverHalf;

    private Memory _memory;
    private LogicGraph _roomGraph;

    public CellNode CurrentCell => _roomGraph.CurrentNode as CellNode;
    
    private Room(Location location, RoomEntry roomEntry, int ladder)
    {
        _location = location;
        _roomEntry = roomEntry;
        _ladder = ladder;
        _isOverHalf = Map.IsOverHalfFromLadder[ladder];
        
        _memory = new Memory();

        ResetRoomGraph();
    }

    private void ResetRoomGraph()
    {
        _roomGraph = _roomEntry.RoomGraph;
        _roomGraph.Reset();
        foreach (Node node in _roomGraph.Nodes)
        {
            if (node is CellNode cellNode)
                cellNode.Reset();
            if (node is CLNode clNode)
                clNode.Reset();
        }
        // _roomGraph.Blackboard.SetLocalValue(_roomGraph, "Ladder", _ladder);
    }

    public static Room CreateRoom(Location location, RoomEntry roomEntry, int ladder)
    {
        return new Room(location, roomEntry, ladder);
    }

    public static Room CreateRoom(RoomEntry roomEntry, int ladder)
    {
        return new Room(null, roomEntry, ladder);
    }

    public Location Location => _location;

    public int GetLadder()
        => _ladder;

    public bool IsOverHalf()
        => _isOverHalf;

    public bool IsFinished()
        => CurrentCell == null;

    public void Step()
        => _roomGraph.Step();

    public void ReceiveSignal(Signal signal)
    {
        bool cellIsEnded = CurrentCell.ReceiveSignal(signal);
        if (cellIsEnded)
            Step();
    }
}