
using Cysharp.Threading.Tasks;

public class GainMaxHealthProcedureDefinition : ProcedureDefinition
{
    public int Value;

    public GainMaxHealthProcedureDefinition(int value)
    {
        Value = value;
    }

    public override async UniTask Cast(CastDetails castDetails)
        => castDetails.Caster.MaxHp += Value;

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        Description description = new();
        description.Sb.Append(PostCondDefinition.Description);
        description.Sb.Append($"气血上限+{Value}");
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