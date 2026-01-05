
public class RoomChangedDetails : RunClosureDetails
{
    public Room ToRoom;

    public RoomChangedDetails(Room toRoom)
    {
        ToRoom = toRoom;
    }
}
