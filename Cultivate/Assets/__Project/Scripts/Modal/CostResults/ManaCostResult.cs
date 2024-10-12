
using Cysharp.Threading.Tasks;

public class ManaCostResult : CostResult
{
    public ManaCostResult(int value) : base(value)
    {
    }
    
    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Mana;
    
    public override async UniTask WillCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.WIL_MANA_COST, this);
    }

    public override async UniTask ApplyCost()
    {
        bool manaSufficient = Entity.GetStackOfBuff("灵气") >= Value;
        if (manaSufficient)
        {
            await Entity.TryConsumeProcedure("灵气", Value);
        }
        else
        {
            await Env.ManaShortageProcedure(this);
            await Entity.CastProcedure(Entity.ManaShortageAction);
        }
        
        Blocking = !manaSufficient;
    }

    public override async UniTask DidCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_MANA_COST, this);
    }
}
