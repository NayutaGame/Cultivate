
public class SelectedMapNodeSignal : Signal
{
    public MapNode MapNode;
    public int Index;

    private SelectedMapNodeSignal(MapNode mapNode, int index)
    {
        MapNode = mapNode;
        Index = index;
    }

    public static SelectedMapNodeSignal FromMapNode(MapNode mapNode)
    {
        int mapNodeIndex = RunManager.Instance.Environment.GetIndexOfMapNode(mapNode);
        return new SelectedMapNodeSignal(mapNode, mapNodeIndex);
    }
}