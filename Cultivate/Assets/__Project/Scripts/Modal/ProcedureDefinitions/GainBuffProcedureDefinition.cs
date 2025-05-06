
using Cysharp.Threading.Tasks;

public class GainBuffProcedureDefinition : ProcedureDefinition
{
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;
    public StageClosure[] Closures;
    public bool Induced;

    public GainBuffProcedureDefinition(
        BuffEntry buffEntry,
        int stack = 1,
        bool recursive = true,
        StageClosure[] closures = null,
        bool induced = false)
    {
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
        Closures = closures;
        Induced = induced;
    }

    public GainBuffDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster,
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
            description.Sb.Append("获得");
        }
        else
        {
            description.Sb.Append("遭受");
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
        GainBuffProcedureDefinition pd = procedureDefinition as GainBuffProcedureDefinition;
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