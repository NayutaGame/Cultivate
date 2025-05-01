
using System;
using Cysharp.Threading.Tasks;

public abstract class ProcedureDefinition
{
    private Func<ProcedureDefinition, CostResult, CastResult, Description> _getDescription;
    
    public ProcedureDefinition()
    {
        _getDescription = DefaultGetDescription;
    }
    
    public virtual async UniTask Cast(StageEnvironment env, CastDetails castDetails)
    {
        
    }

    public Description GetDescription(CostResult costResult, CastResult castResult)
        => _getDescription(this, costResult, castResult);

    public ProcedureDefinition SetDescription(Func<ProcedureDefinition, CostResult, CastResult, Description> getDescription)
    {
        _getDescription = getDescription;
        return this;
    }

    public abstract Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult);
}