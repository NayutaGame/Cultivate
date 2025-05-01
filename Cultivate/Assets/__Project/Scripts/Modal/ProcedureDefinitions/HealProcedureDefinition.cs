
using Cysharp.Threading.Tasks;

public class HealProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Penetrate;
    public StageClosure[] Closures;
    public bool Induced;

    public HealProcedureDefinition(int value,
        bool penetrate = false,
        StageClosure[] closures = null,
        bool induced = false)
    {
        Value = value;
        Penetrate = penetrate;
        Closures = closures;
        Induced = induced;
    }
    
    public HealDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster,
            value: Value,
            penetrate: Penetrate,
            initiator: d.Skill,
            castResult: d.CastResult,
            closures: Closures,
            induced: Induced);

    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
        => await env.HealProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        Description description = new();
        description.Sb.Append($"气血+{Value}");
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
        HealProcedureDefinition pd = procedureDefinition as HealProcedureDefinition;
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