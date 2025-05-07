
using Cysharp.Threading.Tasks;

public class RemoveArmorProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Induced;

    public RemoveArmorProcedureDefinition(int value,
        bool induced = false)
    {
        Value = value;
        Induced = induced;
    }

    public LoseArmorDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: Value,
            listener: d.Skill,
            closures: Closures,
            castResult: d.CastResult,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.LoseArmorProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        Description description = new();
        description.Sb.Append(PostCondDefinition.Description);
        description.Sb.Append($"施加{Value}破甲");
        
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
}