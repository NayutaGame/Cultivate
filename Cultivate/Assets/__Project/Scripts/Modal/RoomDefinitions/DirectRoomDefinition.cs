
using System;
using UnityEngine;

[Serializable]
public class DirectRoomDefinition : RoomDefinition, ISerializationCallbackReceiver
{
    [SerializeField] private RoomEntry _roomEntry;
    
    public DirectRoomDefinition(int ladder, string roomName, Func<Profile, RunEnvironment, bool> pred = null) :
        this(ladder, Encyclopedia.RoomCategory.FromName(roomName), pred) { }

    public DirectRoomDefinition(int ladder, RoomEntry roomEntry, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
        _roomEntry = roomEntry;
    }
    
    public override RoomEntry Draw(Map map, Room room)
    {
        return _roomEntry;
    }

    public override string GetTitle()
        => _roomEntry.GetName();

    public override SpriteEntry GetSprite()
        => Encyclopedia.SpriteCategory.FromName("AdventureRoomIcon");

    public override Description GetDescription()
        => new("将会遭遇事件");
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _roomEntry = string.IsNullOrEmpty(_roomEntry.GetId()) ? null : Encyclopedia.RoomCategory.FromId(_roomEntry.GetId());
    }
}
