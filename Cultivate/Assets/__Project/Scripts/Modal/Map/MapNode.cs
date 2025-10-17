
public class MapNode
{
    private MapNodeEntry _entry;
    private NPC _visitor;
    
    public MapNode(MapNodeEntry entry)
    {
        _entry = entry;
        _visitor = null;
    }

    public MapNodeEntry Entry => _entry;
}