
public class DirectRoomDescriptor : RoomDescriptor
{
    private RoomEntry _roomEntry;

    public DirectRoomDescriptor(int ladder, RoomEntry roomEntry) : base(ladder)
    {
        _roomEntry = roomEntry;
    }
    
    public override void Draw(Map map, Room room)
    {
        room.Entry = _roomEntry;
    }
}
