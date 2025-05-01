
using Cysharp.Threading.Tasks;

public class SetActionPointProcedureDefinition : ProcedureDefinition
{
    public int ActionPoint;
    
    public SetActionPointProcedureDefinition(int actionPoint)
    {
        ActionPoint = actionPoint;
    }
    
    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
    {
        castDetails.Caster.SetActionPoint(ActionPoint);
    }
    
    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        Description description = new();
        description.Sb.Append($"二动");
        // if (Closures != null)
        //     foreach (StageClosure c in Closures)
        //     {
        //         Description closureDescription = c.Description;
        //         // closureDescription.ApplyReplaceValues(castResult);
        //         // closureDescription.ApplyCastResult(castResult, c.Key);
        //         description.Sb.Append(closureDescription);
        //     }
        return description;
    }
}