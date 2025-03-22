
using System;

[Serializable]
public class AscensionRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return "突破境界";
    }

    public AscensionRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }

    public override SpriteEntry GetSprite()
        => "AscensionRoomIcon";

    public override string GetDescription()
        => "将会突破境界";
}
