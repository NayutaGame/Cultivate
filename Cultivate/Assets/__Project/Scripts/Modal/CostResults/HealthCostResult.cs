
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
        await Env.ClosureDict.SendEvent(StageClosureDict.WIL_HEALTH_COST, this);
    }
    
    public override async Task ApplyCost()
    {
        Env.Result.TryAppend($"{Entity.GetName()}消耗了{Value}气血，以使用{Skill.Entry.GetName()}\n");
        await Env.LoseHealthProcedure(Entity, Value, induced: true);
    }

    public override async Task DidCostEvent()
    {
        await Env.ClosureDict.SendEvent(StageClosureDict.DID_HEALTH_COST, this);
    }
}
