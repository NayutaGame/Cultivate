
using System;

[Serializable]
public class AscensionRoomDefinition : RoomDefinition
{
    public override LegacyRoomEntry Draw(Map map, LegacyRoom room)
    {
        return Encyclopedia.LegacyRoomCategory.FromName("突破境界");
    }

    public AscensionRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }

    public override string GetTitle()
        => "突破房间";

    public override SpriteEntry GetSprite()
        => Encyclopedia.SpriteCategory.FromName("AscensionRoomIcon");

    public override Description GetDescription()
        => new("将会突破境界");
}
