
public class AscensionRoomDescriptor : RoomDescriptor
{
    public override void Draw(Map map, Room room)
    {
        room.Entry = "突破境界";
    }

    public AscensionRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
