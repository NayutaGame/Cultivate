
using System;
using PuppyDragon.uNody.Logic;

public class LocationEntry : Entry
{
    [NonSerialized] private RoomEntry _roomFromVoid;
    [NonSerialized] private RoomEntry _roomFromFirstStep;
    [NonSerialized] private RoomEntry[] _roomsFromLocation;
    
    public LocationEntry(string id, string name) : base(id, name)
    {
    }

    public override void Init()
    {
        base.Init();

        _roomFromVoid = Encyclopedia.RoomCategory.FromName($"Void{GetName()}");
        _roomFromFirstStep = Encyclopedia.RoomCategory.FromName($"FirstStep{GetName()}");
        _roomsFromLocation = new RoomEntry[]
        {
            Encyclopedia.RoomCategory.FromName($"Location{GetName()}1"),
            Encyclopedia.RoomCategory.FromName($"Location{GetName()}2"),
            Encyclopedia.RoomCategory.FromName($"Location{GetName()}3"),
        };
    }

    public RoomEntry RoomEntryFromVoid => _roomFromVoid;
    public RoomEntry RoomEntryFromFirstStep => _roomFromFirstStep;
    public RoomEntry[] RoomEntriesFromLocation => _roomsFromLocation;
}
