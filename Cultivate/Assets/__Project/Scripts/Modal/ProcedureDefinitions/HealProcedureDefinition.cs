
using Cysharp.Threading.Tasks;

public class HealProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Penetrate;
    public bool Induced;

    public HealProcedureDefinition(int value,
        bool penetrate = false,
        bool induced = false)
    {
        Value = value;
        Penetrate = penetrate;
        Induced = induced;
    }
    
    public HealDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster,
            value: Value,
            penetrate: Penetrate,
            listener: d.Skill,
            castResult: d.CastResult,
            closures: Closures,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.HealProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        Description description = new();
        description.Sb.Append(PostCondDefinition.Description);
        description.Sb.Append($"气血+{Value}");
        if (Closures != null)
            foreach (StageClosure c in Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                // closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, this);
        
        return description;
    }

    public static Description OnlyClosure(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        HealProcedureDefinition pd = procedureDefinition as HealProcedureDefinition;
        Description description = new();
        description.Sb.Append(pd.PostCondDefinition.Description);
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                // closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
        
        return description;
    }
}