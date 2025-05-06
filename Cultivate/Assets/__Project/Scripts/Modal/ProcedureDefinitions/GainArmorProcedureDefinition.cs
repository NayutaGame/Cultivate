
using Cysharp.Threading.Tasks;

public class GainArmorProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public StageClosure[] Closures;
    public bool Induced;

    public GainArmorProcedureDefinition(int value, 
        StageClosure[] closures = null,
        bool induced = false)
    {
        Value = value;
        Closures = closures;
        Induced = induced;
    }
    
    public GainArmorDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster,
            value: Value,
            listener: d.Skill,
            castResult: d.CastResult,
            closures: Closures,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.GainArmorProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        Description description = new();

        description.Sb.Append(PostCondDefinition.Description);
        description.Sb.Append($"护甲+{Value}");
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
        GainArmorProcedureDefinition pd = procedureDefinition as GainArmorProcedureDefinition;
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