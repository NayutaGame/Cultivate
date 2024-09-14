
public class RestRoomDescriptor : RoomDescriptor
{
    public override void Draw(Map map, Room room)
    {
        room.Entry = "休息";
    }

    public RestRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
