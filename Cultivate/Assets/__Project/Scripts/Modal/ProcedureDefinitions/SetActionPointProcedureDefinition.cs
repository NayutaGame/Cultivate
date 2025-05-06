
using Cysharp.Threading.Tasks;

public class SetActionPointProcedureDefinition : ProcedureDefinition
{
    public int ActionPoint;
    
    public SetActionPointProcedureDefinition(int actionPoint)
    {
        ActionPoint = actionPoint;
    }
    
    public override async UniTask Cast(CastDetails castDetails)
        => castDetails.Caster.SetActionPoint(ActionPoint);
    
    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        Description description = new();
        description.Sb.Append(PostCondDefinition.Description);
        description.Sb.Append($"二动");
        // if (Closures != null)
        //     foreach (StageClosure c in Closures)
        //     {
        //         Description closureDescription = c.Description;
        //         // closureDescription.ApplyReplaceValues(castResult);
        //         // closureDescription.ApplyCastResult(castResult, c.Key);
        //         description.Sb.Append(closureDescription);
        //     }
        
        description.ApplyStyle(castResult, this);
        
        return description;
    }
}