
using System;

public class AdventureRoomDescriptor : RoomDescriptor
{
    public override void Draw(Map map, Room room)
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

        room.Entry = entry;
    }

    public AdventureRoomDescriptor(int ladder) : base(ladder)
    {
    }
}
