
using Cysharp.Threading.Tasks;

public class GiveArmorProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public StageClosure[] Closures;
    public bool Induced;

    public GiveArmorProcedureDefinition(int value, 
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
            tgt: d.Caster.Opponent(),
            value: Value,
            listener: d.Skill,
            castResult: d.CastResult,
            closures: Closures,
            induced: Induced);

    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
        => await env.GainArmorProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        Description description = new();

        description.Sb.Append(PostCondDefinition.Description);
        description.Sb.Append($"给予{Value}护甲");
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

    public static Description OnlyClosure(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        GiveArmorProcedureDefinition pd = procedureDefinition as GiveArmorProcedureDefinition;
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