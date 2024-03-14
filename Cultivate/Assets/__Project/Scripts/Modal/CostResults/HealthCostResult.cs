
using System.Threading.Tasks;

public class HealthCostResult : CostResult
{
    public HealthCostResult(int value) : base(value)
    {
    }

    public override CostDescription.CostType ToType()
        => CostDescription.CostType.Health;

    public override async Task WillCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.WIL_HEALTH_COST, this);
    }
    
    public override async Task ApplyCost()
    {
        Env.Result.TryAppend($"{Entity.GetName()}消耗了{Value}生命，以使用{Skill.GetName()}\n");
        await Env.LoseHealthProcedure(Entity, Value);
    }

    public override async Task DidCostEvent()
    {
        await Env.EventDict.SendEvent(StageEventDict.DID_HEALTH_COST, this);
    }
}
