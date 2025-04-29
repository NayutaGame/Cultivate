
using Cysharp.Threading.Tasks;

public class AttackProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public int Times;
    public WuXing? WuXing;
    public bool Recursive;
    public StageClosure[] Closures;
    public bool Induced;

    public AttackProcedureDefinition(int value,
        int times = 1,
        WuXing? wuXing = null,
        bool recursive = true,
        StageClosure[] closures = null,
        bool induced = false)
    {
        Value = value;
        Times = times;
        WuXing = wuXing;
        Recursive = recursive;
        Closures = closures;
        Induced = induced;
    }

    public AttackDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: Value,
            times: Times,
            initiator: d.Skill,
            wuxing: WuXing ?? d.Skill.Entry.WuXing,
            crit: false,
            lifeSteal: false,
            penetrate: false,
            doesntConsumeJianYi: false,
            shatter: false,
            evade: false,
            recursive: Recursive,
            castResult: d.CastResult,
            closures: Closures,
            induced: Induced);

    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
    {
        if (Closures != null)
            foreach (StageClosure closure in Closures)
            {
                if (closure.Description == null)
                    return;
                castDetails.CastResult.Append(closure.Key, false);
            }
        await env.AttackProcedure(GetDetailsFromCastDetails(castDetails));
    }

    public override Description GetDescription(CostResult costResult, CastResult castResult)
    {
        Description description = new();
        if (Times > 1)
            description.Sb.Append($"{Value}攻x{Times}".ApplyAttack());
        else
            description.Sb.Append($"{Value}攻".ApplyAttack());
        if (Closures != null)
            foreach (StageClosure c in Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        return description;
    }
}