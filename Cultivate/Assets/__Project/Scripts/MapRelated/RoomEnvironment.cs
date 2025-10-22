
using System.Collections.Generic;

public class RoomEnvironment
{
    private MapNode _mapNode;
    private JingJie _jingJie;
    private int _ladder;
    private RunEntity _home;

    private Memory _memory;
    private List<RoomScript> _scripts;
    private int _index;

    private Cell _cell;
    
    private RoomEnvironment(MapNode mapNode, JingJie jingJie, int ladder, RunEntity home)
    {
        _mapNode = mapNode;
        _jingJie = jingJie;
        _ladder = ladder;
        _home = home;

        _memory = new Memory();
        _scripts = _mapNode.Entry._scripts;
    }

    public static RoomEnvironment Create(MapNode mapNode, JingJie jingJie, int ladder, RunEntity home)
    {
        return new RoomEnvironment(mapNode, jingJie, ladder, home);
    }

    public void Interpret()
    {
        
    }
}