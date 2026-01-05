
using System;

[Serializable]
public class SuccessRoomDefinition : RoomDefinition
{
    public override LegacyRoomEntry Draw(Map map, LegacyRoom room)
    {
        return Encyclopedia.LegacyRoomCategory.FromName("胜利");
    }

    public SuccessRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }

    public override string GetTitle()
        => "胜利房间";

    public override SpriteEntry GetSprite()
        => Encyclopedia.SpriteCategory.FromName("AscensionRoomIcon");

    public override Description GetDescription()
        => new("将会取得游戏胜利");
}
