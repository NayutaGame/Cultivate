
using System;
using System.Threading.Tasks;

public class HealthCostResult : CostResult
{
    public HealthCostResult(int value) : base(value)
    {
    }

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

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromValue(int value)
        => async (env, entity, skill, recursive) => new HealthCostResult(value);
    
    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromDj(Func<int, int> dj)
        => async (env, entity, skill, recursive) => new HealthCostResult(dj(skill.Dj));

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> FromJiaShi(Func<bool, int> jiaShi)
        => async (env, entity, skill, recursive) =>
        {
            bool j = await entity.ToggleJiaShiProcedure();
            return new HealthCostResult(jiaShi(j));
        };
}
