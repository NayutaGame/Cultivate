
using System;
using UnityEngine;

[Serializable]
public class DirectRoomDescriptor : RoomDescriptor, ISerializationCallbackReceiver
{
    [SerializeField] private RoomEntry _roomEntry;

    public DirectRoomDescriptor(int ladder, RoomEntry roomEntry) : base(ladder)
    {
        _roomEntry = roomEntry;
    }
    
    public override RoomEntry Draw(Map map, Room room)
    {
        return _roomEntry;
    }

    public override SpriteEntry GetSprite()
        => "AdventureRoomIcon";

    public override string GetDescription()
        => "将会遭遇事件";
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _roomEntry = string.IsNullOrEmpty(_roomEntry.GetName()) ? null : Encyclopedia.RoomCategory[_roomEntry.GetName()];
    }
}
