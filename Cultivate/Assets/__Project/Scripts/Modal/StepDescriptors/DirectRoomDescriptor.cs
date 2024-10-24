
public class DirectRoomDescriptor : RoomDescriptor
{
    private RoomEntry _roomEntry;

    public DirectRoomDescriptor(int ladder, RoomEntry roomEntry) : base(ladder)
    {
        _roomEntry = roomEntry;
    }
    
    public override RoomEntry Draw(Map map, Room room)
    {
        return _roomEntry;
    }

    public override SpriteEntry GetSprite()
        => "AdventureRoomIcon";

    public override string GetDescription()
        => "将会遭遇事件";
}
