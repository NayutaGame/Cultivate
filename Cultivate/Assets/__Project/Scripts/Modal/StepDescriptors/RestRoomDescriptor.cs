
public class RestRoomDescriptor : RoomDescriptor
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return "休息";
    }

    public RestRoomDescriptor(int ladder) : base(ladder)
    {
    }

    public override SpriteEntry GetSprite()
        => "RestRoomIcon";
}
