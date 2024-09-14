
public class BattleStepDescriptor : StepDescriptor
{
    public int _slotCountBefore;
    public int _slotCountAfter;
    public int _baseGoldReward;
    public bool _isBoss;

    public bool ShouldUpdateSlotCount => _slotCountBefore != _slotCountAfter;

    private static readonly bool[] IsBossTable = new bool[]
    {
        false, /*false,*/ true,
        false, false, true,
        false, false, true,
        false, false, true,
        false, false, true,
    };

    public BattleStepDescriptor(int ladder, int slotCountBefore, int slotCountAfter) : base(ladder)
    {
        _slotCountBefore = slotCountBefore;
        _slotCountAfter = slotCountAfter;
        _baseGoldReward = GoldRewardTable[ladder];
        _isBoss = IsBossTable[ladder];
    }
    
    public override RunNode Draw(Map map)
    {
        EntityDescriptor d = new EntityDescriptor(Ladder);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        return new BattleRunNode(entity);
    }
}
