
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

    public override string GetTitle()
        => "休息房间";

    public override SpriteEntry GetSprite()
        => "RestRoomIcon";

    public override Description GetDescription()
        => new("可以休息");
}
