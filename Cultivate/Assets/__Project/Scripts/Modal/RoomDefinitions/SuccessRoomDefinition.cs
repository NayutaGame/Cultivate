
using System;

[Serializable]
public class SuccessRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return "胜利";
    }

    public SuccessRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }

    public override SpriteEntry GetSprite()
        => "AscensionRoomIcon";

    public override string GetDescription()
        => "将会取得游戏胜利";
}
