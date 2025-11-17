
using System;

[Serializable]
public class EncounterRoomDefinition : RoomDefinition
{
    public override LegacyRoomEntry Draw(Map map, Room room)
    {
        map.RoomPool.TryPopItem(out LegacyRoomEntry entry, pred: e => e.CanCreate(map, room));
        return entry;
    }

    public EncounterRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred) : base(ladder, pred)
    {
    }

    public override string GetTitle()
        => "相遇房间";

    public override SpriteEntry GetSprite()
        => Encyclopedia.SpriteCategory.FromName("EncounterRoomIcon");

    public override Description GetDescription()
        => new("将会遭遇其他角色");
}
