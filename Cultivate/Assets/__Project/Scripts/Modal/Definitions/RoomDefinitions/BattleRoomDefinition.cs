
using System;
using UnityEngine;

[Serializable]
public class BattleRoomDefinition : RoomDefinition, ISerializationCallbackReceiver
{
    private static readonly bool[] IsBossTable = new bool[]
    {
        false, /*false,*/ true,
        false, false, true,
        false, false, true,
        false, false, true,
        false, false, true,
    };
    
    private static readonly string[] SpriteTable = new string[]
    {
        "UnderlingRoomIcon", /*"EliteRoomIcon",*/ "BossRoomIcon",
        "UnderlingRoomIcon", "EliteRoomIcon", "BossRoomIcon",
        "UnderlingRoomIcon", "EliteRoomIcon", "BossRoomIcon",
        "UnderlingRoomIcon", "EliteRoomIcon", "BossRoomIcon",
        "UnderlingRoomIcon", "EliteRoomIcon", "BossRoomIcon",
    };
    
    [SerializeField] public int _slotCountBefore;
    [SerializeField] public int _slotCountAfter;
    
    [NonSerialized] public bool _isBoss;
    [NonSerialized] private SpriteEntry _spriteEntry;

    public BattleRoomDefinition(int ladder, int slotCountBefore, int slotCountAfter, Func<Profile, RunEnvironment, bool> pred = null) : base(ladder, pred)
    {
        _slotCountBefore = slotCountBefore;
        _slotCountAfter = slotCountAfter;
        
        _isBoss = IsBossTable[ladder];
        _spriteEntry = Encyclopedia.SpriteCategory.FromName(SpriteTable[ladder]);
    }
    
    public bool ShouldUpdateSlotCount => _slotCountBefore != _slotCountAfter;
    
    public override RoomEntry Draw(Map map, Room room)
    {
        EntityDescriptor d = new EntityDescriptor(Ladder);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        room.SetPredrewRunEntity(entity);
        return Encyclopedia.RoomCategory.FromName("战斗");
    }

    public override string GetTitle()
        => "战斗房间";

    public override SpriteEntry GetSprite()
        => _spriteEntry;

    public override Description GetDescription()
        => new("将会发生战斗");
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _isBoss = IsBossTable[Ladder];
        _spriteEntry = Encyclopedia.SpriteCategory.FromName(SpriteTable[Ladder]);
    }
}
