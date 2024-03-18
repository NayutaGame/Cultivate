
public class DrawDescriptor
{
    public enum NodeType
    {
        Battle,
        Adventure,
        Rest,
        Shop,
        Boss,
        Ascension,
    }

    private NodeType _nodeType;
    private NodeEntry _priority;

    public DrawDescriptor(NodeType nodeType, NodeEntry priority = null)
    {
        _nodeType = nodeType;
        _priority = priority;
    }
}
