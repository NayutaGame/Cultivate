
using System;

[Serializable]
public class RestRoomDefinition : RoomDefinition
{
    public override LegacyRoomEntry Draw(Map map, LegacyRoom room)
    {
        return Encyclopedia.LegacyRoomCategory.FromName("休息");
    }

    public RestRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }

    public override string GetTitle()
        => "休息房间";

    public override SpriteEntry GetSprite()
        => Encyclopedia.SpriteCategory.FromName("RestRoomIcon");

    public override Description GetDescription()
        => new("可以休息");
}
