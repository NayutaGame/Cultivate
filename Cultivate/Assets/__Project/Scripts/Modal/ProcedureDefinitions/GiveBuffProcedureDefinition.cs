
using Cysharp.Threading.Tasks;

public class GiveBuffProcedureDefinition : ProcedureDefinition
{
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;
    public bool Induced;

    public GiveBuffProcedureDefinition(
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
        => new(
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            buffEntry: BuffEntry,
            stack: Stack,
            recursive: Recursive,
            listener: d.Skill,
            castResult: d.CastResult,
            closures: Closures,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.GainBuffProcedure(GetDetailsFromCastDetails(castDetails));

    public override Description DefaultGetDescription(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        Description description = new();

        description.Sb.Append(PostCondDefinition.Description);
        if (BuffEntry.Friendly)
        {
            description.Sb.Append("给予");
        }
        else
        {
            description.Sb.Append("施加");
        }

        description.Sb.Append($"{Stack}{BuffEntry.GetName()}");
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
        GiveBuffProcedureDefinition pd = procedureDefinition as GiveBuffProcedureDefinition;
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