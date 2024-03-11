
using System;
using System.Threading.Tasks;

public abstract class CostResult
{
    public StageEnvironment Env;
    public StageEntity Entity;
    public StageSkill Skill;

    public bool Blocking = false;

    public virtual async Task WillCostEvent()
    {
    }

    public virtual async Task ApplyCost()
    {
    }

    public virtual async Task DidCostEvent()
    {
        
    }

    public static async Task<CostResult> FromEnvironment(StageEnvironment env, StageEntity entity, StageSkill skill)
    {
        CostResult result = await skill.CostProcedure(env, entity);
        return result;
    }

    public static Func<StageEnvironment, StageEntity, StageSkill, bool, Task<CostResult>> Empty
        => async (env, entity, skill, recursive) => new EmptyCostResult();
}
