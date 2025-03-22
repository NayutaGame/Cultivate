
using System;

[Serializable]
public class RestRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        return "休息";
    }

    public RestRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }

    public override SpriteEntry GetSprite()
        => "RestRoomIcon";

    public override string GetDescription()
        => "可以休息";
}
