
using System.Threading.Tasks;

public class ManaCostResult : CostResult
{
    public ManaCostResult(int value) : base(value)
    {
    }
    
    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Mana;
    
    public override async Task WillCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.WIL_MANA_COST, this);
    }

    public override async Task ApplyCost()
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

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_MANA_COST, this);
    }
}
