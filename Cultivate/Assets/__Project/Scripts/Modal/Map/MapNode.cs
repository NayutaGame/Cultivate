
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

        bool isFirstStep = false;
        if (isFirstStep)
        {
            AddFromFirstStep(roomOptions);
        }
        else
        {
            // RoomOptionFromCausality
            AddFromCharacter(roomOptions);
            AddFromVisitor(roomOptions);
            AddFromLocation(roomOptions);
            AddFromVoid(roomOptions);
        }

        return roomOptions;
    }

    private void AddFromCharacter(List<RoomOption> roomOptions)
    {
        CharacterEntry characterEntry = RunManager.Instance.Environment.Character.CharacterEntry;
        characterEntry.RoomsFromCharacter.Do(roomEntry =>
        {
            // if roomEntry is valid
            roomOptions.Add(new(roomEntry, $"{characterEntry.GetName()}对这里感到有兴趣"));
        });
    }

    private void AddFromVisitor(List<RoomOption> roomOptions)
    {
        if (_visitor == null)
            return;
        
        roomOptions.Add(new(_visitor.CharacterEntry.RoomFromVisitor, $"对{_visitor.CharacterEntry.GetName()}感到有兴趣"));
    }

    private void AddFromLocation(List<RoomOption> roomOptions)
    {
        Entry.RoomEntriesFromLocation.Do(roomEntry =>
        {
            // if roomEntry is valid
            roomOptions.Add(new(roomEntry, $"对{Entry.GetName()}感到有兴趣"));
        });
    }

    private void AddFromVoid(List<RoomOption> roomOptions)
    {
        roomOptions.Add(new(Entry.RoomEntryFromVoid, "正常进入"));
    }

    private void AddFromFirstStep(List<RoomOption> roomOptions)
    {
        roomOptions.Add(new(Entry.RoomEntryFromVoid, "以这里开始修行之旅"));
    }
}