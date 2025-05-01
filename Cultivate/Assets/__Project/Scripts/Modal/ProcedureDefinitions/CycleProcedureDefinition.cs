
using Cysharp.Threading.Tasks;

public class CycleProcedureDefinition : ProcedureDefinition
{
    public WuXing WuXing;
    public bool Rotate;
    public int Gain;
    public int Recover;
    public StageClosure[] Closures;
    public bool Induced;
    
    public CycleProcedureDefinition(
        WuXing wuXing,
        bool rotate = true,
        int gain = 0,
        int recover = 0,
        StageClosure[] closures = null,
        bool induced = false)
    {
        WuXing = wuXing;
        Rotate = rotate;
        Gain = gain;
        Recover = recover;
        Closures = closures;
        Induced = induced;
    }
    
    public CycleDetails GetDetailsFromCastDetails(CastDetails d)
        => new(d.Caster, Rotate, WuXing, Gain, Recover, d.Skill, Closures, d.CastResult, Induced);

    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
    {
        await env.CycleProcedure(GetDetailsFromCastDetails(castDetails));
    }

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult,
        CastResult castResult)
    {
        Description description = new();
        description.Sb.Append($"{WuXing._elementaryBuff}+{Gain}");
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

    public static Description OnlyClosure(ProcedureDefinition procedureDefinition, CostResult costResult,
        CastResult castResult)
    {
        CycleProcedureDefinition pd = procedureDefinition as CycleProcedureDefinition;
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