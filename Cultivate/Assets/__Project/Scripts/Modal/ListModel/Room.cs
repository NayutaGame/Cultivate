
using System.Collections.Generic;

public class Room
{
    public enum RoomState
    {
        Past,
        Curr,
        Future,
    }

    private RoomState _state;
    public RoomState State => _state;
    public void SetState(RoomState state) => _state = state;

    private RoomDescriptor _descriptor;
    public RoomDescriptor Descriptor => _descriptor;

    public RoomEntry Entry;

    private Dictionary<string, object> _details;
    public Dictionary<string, object> Details => _details ??= new();

    public Room(RoomDescriptor descriptor)
    {
        _descriptor = descriptor;
        _state = RoomState.Future;
    }

    public int Ladder => _descriptor.Ladder;

    public bool HasEntry()
        => Entry != null;

    public void DrawEntry(Map map)
    {
        Entry = _descriptor.Draw(map, this);
    }

    public void CreatePanel(Map map)
    {
        map.Panel = Entry.Create(map, this);
    }
}
