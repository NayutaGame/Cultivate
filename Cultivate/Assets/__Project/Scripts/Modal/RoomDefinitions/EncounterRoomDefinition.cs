
using System;

[Serializable]
public class EncounterRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        map.RoomPool.TryPopItem(out RoomEntry entry, pred: e => e.CanCreate(map, room));
        return entry;
    }

    public EncounterRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred) : base(ladder, pred)
    {
    }

    public override SpriteEntry GetSprite()
        => "EncounterRoomIcon";

    public override string GetDescription()
        => "将会遭遇其他角色";
}
