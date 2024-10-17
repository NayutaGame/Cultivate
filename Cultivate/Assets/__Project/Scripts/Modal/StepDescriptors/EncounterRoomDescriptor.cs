
public class EncounterRoomDescriptor : RoomDescriptor
{
    public override RoomEntry Draw(Map map, Room room)
    {
        map.RoomPool.TryPopItem(out RoomEntry entry, pred: e => e.CanCreate(map, room));
        return entry;
    }

    public EncounterRoomDescriptor(int ladder) : base(ladder)
    {
    }

    public override SpriteEntry GetSprite()
        => "EncounterRoomIcon";
}
