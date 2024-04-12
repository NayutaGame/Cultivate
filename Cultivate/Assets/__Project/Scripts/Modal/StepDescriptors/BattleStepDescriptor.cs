
public class BattleStepDescriptor : StepDescriptor
{
    public int _ladder;
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

    public BattleStepDescriptor(int ladder, int slotCountBefore, int slotCountAfter)
    {
        _ladder = ladder;
        _slotCountBefore = slotCountBefore;
        _slotCountAfter = slotCountAfter;
        _baseGoldReward = GoldRewardTable[_ladder];
        _isBoss = IsBossTable[_ladder];
    }
    
    public override void Draw(Map map)
    {
        map.CurrStepItem._nodes.Clear();

        map.Ladder = _ladder;
        
        DrawEntityDetails d = new DrawEntityDetails(_ladder);
        map.EntityPool.TryDrawEntity(out RunEntity entity, d);
        map.CurrStepItem._nodes.Add(new BattleRunNode(entity));
    }
}
