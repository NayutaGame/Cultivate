
using Cysharp.Threading.Tasks;

public abstract class ProcedureDefinition
{
    public virtual async UniTask Cast(StageEnvironment env, CastDetails castDetails)
    {
        
    }

    public abstract Description GetDescription(CostResult costResult, CastResult castResult);
}