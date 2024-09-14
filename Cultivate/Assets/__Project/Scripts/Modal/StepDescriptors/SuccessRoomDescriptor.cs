
public class SuccessRoomDescriptor : RoomDescriptor
{
    public override void Draw(Map map, Room room)
    {
        room.Entry = "胜利";
    }

    public SuccessRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
