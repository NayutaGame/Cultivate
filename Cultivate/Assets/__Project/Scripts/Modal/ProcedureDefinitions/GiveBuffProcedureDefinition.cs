
using System;
using System.Collections.Generic;
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

    protected GiveBuffProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        BuffEntry buffEntry,
        int stack,
        bool recursive,
        bool induced) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
        Induced = induced;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new GiveBuffProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            buffEntry: BuffEntry,
            stack: Stack,
            recursive: Recursive,
            induced: Induced
        );
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
            closures: ClosuresArray,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.GainBuffProcedure(GetDetailsFromCastDetails(castDetails));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        GiveBuffProcedureDefinition pd = procedureDefinition as GiveBuffProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        if (pd.BuffEntry.Friendly)
        {
            description.Sb.Append("给予");
        }
        else
        {
            description.Sb.Append("施加");
        }

        description.Sb.Append($"{pd.Stack}{pd.BuffEntry.GetName()}");
        
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                description.AppendSoftReturn();
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}