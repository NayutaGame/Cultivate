
using System;

[Serializable]
public class AdventureRoomDefinition : RoomDefinition
{
    public override RoomEntry Draw(Map map, Room room)
    {
        Predicate<RoomEntry> pred = e => e.CanCreate(map, room);
        RoomEntry entry;
        
        if (map.InsertedRoomPool.TryPopItem(out entry, pred: pred))
        {
            
        }
        else if (map.RoomPool.TryPopItem(out entry, pred: pred))
        {
            
        }
        else
        {
            entry = Encyclopedia.RoomCategory.FromName("不存在的事件");
        }

        return entry;
    }

    public override string GetTitle()
        => "奇遇房间";

    public override SpriteEntry GetSprite()
        => Encyclopedia.SpriteCategory.FromName("AdventureRoomIcon");

    public override Description GetDescription()
        => new("将会遭遇事件");

    public AdventureRoomDefinition(int ladder, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
    }
}
