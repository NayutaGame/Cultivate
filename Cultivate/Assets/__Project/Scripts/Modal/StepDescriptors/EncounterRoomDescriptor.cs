
public class EncounterRoomDescriptor : RoomDescriptor
{
    public override void Draw(Map map, Room room)
    {
        map.RoomPool.TryPopItem(out RoomEntry entry, pred: e => e.CanCreate(map, room));
        room.Entry = entry;
    }

    public EncounterRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
