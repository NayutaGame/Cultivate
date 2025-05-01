
using Cysharp.Threading.Tasks;

public class RemoveArmorProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Induced;

    public RemoveArmorProcedureDefinition(int value, bool induced)
    {
        Value = value;
        Induced = induced;
    }

    public LoseArmorDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: Value,
            induced: Induced);

    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
        => await env.LoseArmorProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult)
    {
        Description description = new();
        description.Sb.Append($"施加{Value}破甲");
        return description;
    }
}