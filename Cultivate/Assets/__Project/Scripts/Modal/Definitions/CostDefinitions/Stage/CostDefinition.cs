
using System;
using Cysharp.Threading.Tasks;

public abstract class CostDefinition
{
    protected PreCondDefinition PreCondDefinition;
    protected PostCondDefinition PostCondDefinition;
    protected Func<CostDefinition, ResultDict, Description> _getDescription;
    
    public int Value;
    public StageClosure[] Closures;
    
    public CostDefinition(
        int value,
        StageClosure[] closures = null)
    {
        PreCondDefinition = PreCondDefinition.Default;
        PostCondDefinition = PostCondDefinition.Default;
        Value = value;
        Closures = closures ?? Array.Empty<StageClosure>();
    }

    protected CostDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Func<CostDefinition, ResultDict, Description> getDescription,
        int value,
        StageClosure[] closures)
    {
        PreCondDefinition = preCondDefinition;
        PostCondDefinition = postCondDefinition;
        _getDescription = getDescription;
        Value = value;
        Closures = closures;
    }

    public abstract CostDefinition Clone();

    public virtual async UniTask WillCostEvent(CostDetails d) { }

    public virtual async UniTask ApplyCost(CostDetails d) { }

    public virtual async UniTask DidCostEvent(CostDetails d) { }

    public abstract CostDescription GetLiteralCostDescription();
    
    public virtual Description DefaultGetDescription(ResultDict costResult)
    {
        Description description = new();
    
        description.Join(PostCondDefinition.Description);
        
        if (Closures != null)
            foreach (StageClosure c in Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(costResult);
                closureDescription.ApplyResult(costResult, c.Key);
                description.Join(closureDescription);
            }
        
        description.ApplyStyle(costResult, this);
        
        return description;
    }
}