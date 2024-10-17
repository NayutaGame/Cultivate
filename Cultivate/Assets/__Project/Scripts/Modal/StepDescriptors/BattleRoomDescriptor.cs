
public class BattleRoomDescriptor : RoomDescriptor
{
    public int _slotCountBefore;
    public int _slotCountAfter;
    public int _baseGoldReward;
    public bool _isBoss;

    private SpriteEntry _spriteEntry;

    public bool ShouldUpdateSlotCount => _slotCountBefore != _slotCountAfter;

    private static readonly bool[] IsBossTable = new bool[]
    {
        false, /*false,*/ true,
        false, false, true,
        false, false, true,
        false, false, true,
        false, false, true,
    };

    public BattleRoomDescriptor(int ladder, int slotCountBefore, int slotCountAfter) : base(ladder)
    {
        _slotCountBefore = slotCountBefore;
        _slotCountAfter = slotCountAfter;
        _baseGoldReward = GoldRewardTable[ladder];
        _isBoss = IsBossTable[ladder];

        if (ladder % 3 == 0)
            _spriteEntry = "UnderlingRoomIcon";
        else if (ladder % 3 == 1)
            _spriteEntry = "EliteRoomIcon";
        else
            _spriteEntry = "BossRoomIcon";
    }
    
    public override RoomEntry Draw(Map map, Room room)
    {
        EntityDescriptor d = new EntityDescriptor(Ladder);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        room.Details["Entity"] = entity;
        return "战斗";
    }

    public override SpriteEntry GetSprite()
        => _spriteEntry;
}
