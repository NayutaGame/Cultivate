
public class SelectedLocationSignal : Signal
{
    public Location Location;
    public int Index;
    public RoomEntry RoomEntry;

    private SelectedLocationSignal(Location location, int index, RoomEntry roomEntry)
    {
        Location = location;
        Index = index;
        RoomEntry = roomEntry;
    }

    public static SelectedLocationSignal FromLocationAndRoomEntry(Location location, RoomEntry roomEntry)
    {
        int locationIndex = RunManager.Instance.Environment.Map.GetIndexOfLocation(location);
        return new SelectedLocationSignal(location, locationIndex, roomEntry);
    }
}