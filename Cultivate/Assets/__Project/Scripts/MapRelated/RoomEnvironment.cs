
using PuppyDragon.uNody.Logic;

public class RoomEnvironment
{
    private MapNode _mapNode;
    private JingJie _jingJie;
    private int _ladder;
    private RunEntity _home;

    private Memory _memory;
    private LogicGraph _roomGraph;
    private int _index;

    public CellNode CurrentCell => _roomGraph.CurrentNode as CellNode;
    
    private RoomEnvironment(MapNode mapNode, JingJie jingJie, int ladder, RunEntity home)
    {
        _mapNode = mapNode;
        _jingJie = jingJie;
        _ladder = ladder;
        _home = home;

        _memory = new Memory();
        _roomGraph = _mapNode.Entry.RoomGraph;
    }

    public static RoomEnvironment CreateRoom(MapNode mapNode, JingJie jingJie, int ladder, RunEntity home)
    {
        return new RoomEnvironment(mapNode, jingJie, ladder, home);
    }

    public void Step()
    {
        _roomGraph.Step();
    }

    public void ReceiveSignal(Signal signal)
    {
        bool cellIsEnded = CurrentCell.ReceiveSignal(signal);
        if (cellIsEnded)
            Step();
    }

    public bool IsFinished()
        => CurrentCell == null;
}