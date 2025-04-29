
using System.Text;
using Cysharp.Threading.Tasks;

public class GainBuffProcedureDefinition : ProcedureDefinition
{
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;
    public bool Induced;

    public GainBuffProcedureDefinition(
        BuffEntry buffEntry,
        int stack = 1,
        bool recursive = true,
        bool induced = false)
    {
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
        Induced = induced;
    }

    public GainBuffDetails GetDetailsFromCastDetails(CastDetails d)
        => new(d.Caster, d.Caster, BuffEntry, Stack, Recursive, Induced);

    public override async UniTask Cast(StageEnvironment env, CastDetails castDetails)
        => await env.GainBuffProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description GetDescription(CostResult costResult, CastResult castResult)
    {
        Description description = new();
        if (BuffEntry.Friendly)
        {
            description.Sb.Append("获得");
        }
        else
        {
            description.Sb.Append("遭受");
        }

        description.Sb.Append($"{Stack}{BuffEntry.GetName()}");
        return description;
    }
}