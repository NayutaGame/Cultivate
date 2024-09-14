
public class AscensionRoomDescriptor : RoomDescriptor
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return "突破境界";
    }

    public AscensionRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
