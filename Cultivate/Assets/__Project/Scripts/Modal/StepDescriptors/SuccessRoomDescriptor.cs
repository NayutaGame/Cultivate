
public class SuccessRoomDescriptor : RoomDescriptor
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return "胜利";
    }

    public SuccessRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
