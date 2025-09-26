
using System;
using Cysharp.Threading.Tasks;

public class HealthCostDefinition : CostDefinition
{
    public HealthCostDefinition(int value, StageClosure[] closures = null) : base(value, closures) { }

    public HealthCostDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Func<CostDefinition, ResultDict, Description> getDescription,
        int value, StageClosure[] closures) :
        base(preCondDefinition, postCondDefinition, getDescription, value, closures) { }

    public override CostDefinition Clone()
    {
        StageClosure[] clonedClosures = new StageClosure[Closures.Length];
        for (int i = 0; i < Closures.Length; i++)
            clonedClosures[i] = Closures[i];

        return new HealthCostDefinition(
            PreCondDefinition.Clone(),
            PostCondDefinition.Clone(),
            _getDescription,
            Value,
            clonedClosures
        );
    }

    public static Func<int, int, HealthCostDefinition> FromValue(int value)
        => (j, dj) => new(value);
    
    public static Func<int, int, HealthCostDefinition> FromJ(Func<int, int> jFunc)
        => (j, dj) => new(jFunc(j));
    
    public static Func<int, int, HealthCostDefinition> FromDj(Func<int, int> djFunc)
        => (j, dj) => new(djFunc(dj));

    public override async UniTask WillCostEvent(CostDetails d)
    {
        if (Closures != null)
        {
            bool postCond = await PostCondDefinition.GetCond(d);
            if (postCond)
                foreach (StageClosure closure in Closures)
                    if (closure.EventId == StageClosureDict.WIL_HEALTH_COST)
                        await closure.Invoke(d.Skill, d);
            
            d.CostResult.Append(this, postCond);
        }
        
        await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_HEALTH_COST, d);
    }
    
    public override async UniTask ApplyCost(CostDetails d)
    {
        d.Env.Result.TryAppend($"{d.Entity.GetName()}消耗了{d.Value}气血，以使用{d.Skill.Entry.GetName()}\n");
        await d.Env.BurnProcedure(BurnDetails.FromHealthCost(d));
    }

    public override async UniTask DidCostEvent(CostDetails d)
    {
        await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_HEALTH_COST, d);
    }

    public override CostDescription GetLiteralCostDescription()
        => new(CostType.Health, CostState.Normal, Value);
}