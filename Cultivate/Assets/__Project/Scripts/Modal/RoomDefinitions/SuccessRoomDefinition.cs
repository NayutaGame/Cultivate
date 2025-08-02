
using System;

[Serializable]
public class SuccessRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return Encyclopedia.RoomCategory.FromName("胜利");
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
