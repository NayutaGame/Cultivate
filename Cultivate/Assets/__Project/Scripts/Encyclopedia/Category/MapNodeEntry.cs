
using System;
using PuppyDragon.uNody.Logic;

public class MapNodeEntry : Entry
{
    [NonSerialized] private RoomEntry _roomFromVoid;
    [NonSerialized] private RoomEntry[] _roomsFromLocation;
    
    public MapNodeEntry(string id, string name) : base(id, name)
    {
    }

    public override void Init()
    {
        base.Init();

        _roomFromVoid = Encyclopedia.RoomCategory.FromName($"Void{GetName()}");
        _roomsFromLocation = new RoomEntry[]
        {
            Encyclopedia.RoomCategory.FromName($"Location{GetName()}1"),
            Encyclopedia.RoomCategory.FromName($"Location{GetName()}2"),
            Encyclopedia.RoomCategory.FromName($"Location{GetName()}3"),
        };
    }

    public RoomEntry RoomEntryFromVoid => _roomFromVoid;
    public RoomEntry[] RoomEntriesFromLocation => _roomsFromLocation;
}
