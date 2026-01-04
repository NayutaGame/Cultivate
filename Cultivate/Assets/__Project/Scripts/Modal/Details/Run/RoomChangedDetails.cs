
public class RoomChangedDetails : RunClosureDetails
{
    public RoomEnvironment ToRoom;

    public RoomChangedDetails(RoomEnvironment toRoom)
    {
        ToRoom = toRoom;
    }
}
