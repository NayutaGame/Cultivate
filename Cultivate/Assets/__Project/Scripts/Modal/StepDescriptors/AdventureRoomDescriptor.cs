
using System;

[Serializable]
public class AdventureRoomDescriptor : RoomDescriptor
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
            entry = "不存在的事件";
        }

        return entry;
    }

    public override SpriteEntry GetSprite()
        => "AdventureRoomIcon";

    public override string GetDescription()
        => "将会遭遇事件";

    public AdventureRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
