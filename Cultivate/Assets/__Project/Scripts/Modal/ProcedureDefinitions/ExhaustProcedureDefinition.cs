
using Cysharp.Threading.Tasks;

public class ExhaustProcedureDefinition : ProcedureDefinition
{
    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
        => await env.ExhaustProcedure(castDetails.Caster, castDetails.Skill);

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        Description description = new();

        description.Sb.Append(PostCondDefinition.Description);
        description.Sb.Append($"升华");
        // if (Closures != null)
        //     foreach (StageClosure c in Closures)
        //     {
        //         Description closureDescription = c.Description;
        //         closureDescription.ApplyReplaceValues(castResult);
        //         // closureDescription.ApplyCastResult(castResult, c.Key);
        //         description.Sb.Append(closureDescription);
        //     }
        
        description.ApplyStyle(castResult, this);
        
        return description;
    }
}