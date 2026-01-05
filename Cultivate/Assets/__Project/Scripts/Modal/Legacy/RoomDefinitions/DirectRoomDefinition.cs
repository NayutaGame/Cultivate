
using System;
using UnityEngine;

[Serializable]
public class DirectRoomDefinition : RoomDefinition, ISerializationCallbackReceiver
{
    [SerializeField] private LegacyRoomEntry _roomEntry;
    
    public DirectRoomDefinition(int ladder, string roomName, Func<Profile, RunEnvironment, bool> pred = null) :
        this(ladder, Encyclopedia.LegacyRoomCategory.FromName(roomName), pred) { }

    public DirectRoomDefinition(int ladder, LegacyRoomEntry roomEntry, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
        _roomEntry = roomEntry;
    }
    
    public override LegacyRoomEntry Draw(Map map, LegacyRoom room)
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
        _roomEntry = string.IsNullOrEmpty(_roomEntry.GetId()) ? null : Encyclopedia.LegacyRoomCategory.FromId(_roomEntry.GetId());
    }
}
