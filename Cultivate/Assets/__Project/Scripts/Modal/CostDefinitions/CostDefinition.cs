
using System;
using Cysharp.Threading.Tasks;

public abstract class CostDefinition
{
    protected PreCondDefinition PreCondDefinition;
    protected PostCondDefinition PostCondDefinition;
    private Func<CostDefinition, ResultDict, Description> _getDescription;
    
    public int Value;
    public StageClosure[] Closures;
    
    public CostDefinition(
        int value,
        StageClosure[] closures = null)
    {
        PreCondDefinition = PreCondDefinition.Default;
        PostCondDefinition = PostCondDefinition.Default;
        Value = value;
        Closures = closures;
    }

    public virtual async UniTask WillCostEvent(CostDetails d) { }

    public virtual async UniTask ApplyCost(CostDetails d) { }

    public virtual async UniTask DidCostEvent(CostDetails d) { }

    public abstract CostDescription GetLiteralCostDescription();
    
    public virtual Description DefaultGetDescription(ResultDict costResult)
    {
        Description description = new();
    
        description.Sb.Append(PostCondDefinition.Description);
        
        if (Closures != null)
            foreach (StageClosure c in Closures)
            {
                Description closureDescription = c.Description;
                // closureDescription.ApplyReplaceValues(castResult);
                // closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(costResult, this);
        
        return description;
    }
}