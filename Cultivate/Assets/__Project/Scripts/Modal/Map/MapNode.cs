
using System.Collections.Generic;
using CLLibrary;

public class MapNode
{
    private MapNodeEntry _entry;
    private RunNPC _visitor;
    
    public MapNode(MapNodeEntry entry)
    {
        _entry = entry;
        _visitor = null;
    }

    public MapNodeEntry Entry => _entry;

    public List<RoomOption> GetRoomOptions()
    {
        List<RoomOption> roomOptions = new();
        
        // RoomOptionFromCausality
        AddFromCharacter(roomOptions);
        AddFromVisitor(roomOptions);
        AddFromLocation(roomOptions);
        AddFromVoid(roomOptions);

        return roomOptions;
    }

    private void AddFromCharacter(List<RoomOption> roomOptions)
    {
        RunManager.Instance.Environment.Character.CharacterEntry.RoomsFromCharacter.Do(roomEntry =>
        {
            // if roomEntry is valid
            roomOptions.Add(new(roomEntry));
        });
    }

    private void AddFromVisitor(List<RoomOption> roomOptions)
    {
        if (_visitor == null)
            return;
        
        roomOptions.Add(new(_visitor.CharacterEntry.RoomFromVisitor));
    }

    private void AddFromLocation(List<RoomOption> roomOptions)
    {
        Entry.RoomEntriesFromLocation.Do(roomEntry =>
        {
            // if roomEntry is valid
            roomOptions.Add(new(roomEntry));
        });
    }

    private void AddFromVoid(List<RoomOption> roomOptions)
    {
        roomOptions.Add(new(Entry.RoomEntryFromVoid));
    }
}