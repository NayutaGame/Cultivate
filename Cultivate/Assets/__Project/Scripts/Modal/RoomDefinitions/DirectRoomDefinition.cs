
using System;
using UnityEngine;

[Serializable]
public class DirectRoomDefinition : RoomDefinition, ISerializationCallbackReceiver
{
    [SerializeField] private RoomEntry _roomEntry;

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
        => "AdventureRoomIcon";

    public override Description GetDescription()
        => new("将会遭遇事件");
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _roomEntry = string.IsNullOrEmpty(_roomEntry.GetName()) ? null : Encyclopedia.RoomCategory[_roomEntry.GetName()];
    }
}
