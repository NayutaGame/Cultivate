
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
            initiator: d.Skill,
            castResult: d.CastResult,
            closures: Closures,
            induced: Induced);

    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
        => await env.GainArmorProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        Description description = new();
        description.Sb.Append($"护甲+{Value}");
        if (Closures != null)
            foreach (StageClosure c in Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                // closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        return description;
    }

    public static Description OnlyClosure(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        GainArmorProcedureDefinition pd = procedureDefinition as GainArmorProcedureDefinition;
        Description description = new();
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                // closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        return description;
    }
}