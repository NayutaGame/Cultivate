
using System;

[Serializable]
public class SuccessRoomDescriptor : RoomDescriptor
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return "胜利";
    }

    public SuccessRoomDescriptor(int ladder) : base(ladder)
    {
    }

    public override SpriteEntry GetSprite()
        => "AscensionRoomIcon";

    public override string GetDescription()
        => "将会取得游戏胜利";
}
