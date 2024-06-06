
public class BattleStepDescriptor : StepDescriptor
{
    public int _slotCountBefore;
    public int _slotCountAfter;
    public int _baseGoldReward;
    public bool _isBoss;

    public bool ShouldUpdateSlotCount => _slotCountBefore != _slotCountAfter;
    
    private static readonly int[] GoldRewardTable = new int[]
    {
        5, /*11,*/ 31,
        11, 21, 61,
        15, 31, 91,
        21, 41, 121,
        25, 51, 151,
    };

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
    
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();
        
        EntityDescriptor d = new EntityDescriptor(Ladder);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        map.CurrStepItem._nodes.Add(new BattleRunNode(entity, Ladder));
    }
}
