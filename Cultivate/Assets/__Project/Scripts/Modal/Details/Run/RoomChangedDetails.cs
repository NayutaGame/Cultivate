
public class RoomChangedDetails : RunClosureDetails
{
    public Room FromRoom;
    public Room ToRoom;

    public RoomChangedDetails(Room fromRoom, Room toRoom)
    {
        FromRoom = fromRoom;
        ToRoom = toRoom;
    }
}
