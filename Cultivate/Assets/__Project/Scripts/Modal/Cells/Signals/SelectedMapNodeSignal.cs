
public class SelectedMapNodeSignal : Signal
{
    public MapNode MapNode;
    public int Index;
    public RoomEntry RoomEntry;

    private SelectedMapNodeSignal(MapNode mapNode, int index, RoomEntry roomEntry)
    {
        MapNode = mapNode;
        Index = index;
        RoomEntry = roomEntry;
    }

    public static SelectedMapNodeSignal FromMapNodeAndRoomEntry(MapNode mapNode, RoomEntry roomEntry)
    {
        int mapNodeIndex = RunManager.Instance.Environment.Map.GetIndexOfMapNode(mapNode);
        return new SelectedMapNodeSignal(mapNode, mapNodeIndex, roomEntry);
    }
}