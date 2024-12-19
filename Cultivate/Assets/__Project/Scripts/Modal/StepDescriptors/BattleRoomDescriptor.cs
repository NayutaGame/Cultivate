
using System;
using UnityEngine;

[Serializable]
public class BattleRoomDescriptor : RoomDescriptor, ISerializationCallbackReceiver
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
    
    [NonSerialized] public int _baseGoldReward;
    [NonSerialized] public bool _isBoss;
    [NonSerialized] private SpriteEntry _spriteEntry;

    public BattleRoomDescriptor(int ladder, int slotCountBefore, int slotCountAfter) : base(ladder)
    {
        _slotCountBefore = slotCountBefore;
        _slotCountAfter = slotCountAfter;
        
        _baseGoldReward = GoldRewardTable[ladder];
        _isBoss = IsBossTable[ladder];
        _spriteEntry = SpriteTable[ladder];
    }
    
    public bool ShouldUpdateSlotCount => _slotCountBefore != _slotCountAfter;
    
    public override RoomEntry Draw(Map map, Room room)
    {
        EntityDescriptor d = new EntityDescriptor(Ladder);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        room.SetPredrewRunEntity(entity);
        return "战斗";
    }

    public override SpriteEntry GetSprite()
        => _spriteEntry;

    public override string GetDescription()
        => "将会发生战斗";
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        _baseGoldReward = GoldRewardTable[Ladder];
        _isBoss = IsBossTable[Ladder];
        _spriteEntry = SpriteTable[Ladder];
    }
}
