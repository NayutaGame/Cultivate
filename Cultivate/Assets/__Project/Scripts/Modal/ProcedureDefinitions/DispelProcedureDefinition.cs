
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class DispelProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Induced;

    public DispelProcedureDefinition(int value,
        bool induced = false)
    {
        Value = value;
        Induced = induced;
    }

    protected DispelProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        int value,
        bool induced) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        Value = value;
        Induced = induced;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new DispelProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            value: Value,
            induced: Induced
        );
    }

    public DispelDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            entity: d.Caster,
            value: Value,
            listener: d.Skill,
            closures: ClosuresArray,
            castResult: d.CastResult,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.DispelProcedure(GetDetailsFromCastDetails(castDetails));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        DispelProcedureDefinition pd = procedureDefinition as DispelProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        if (pd.Value > 0)
            description.Sb.Append($"净化{pd.Value}");
        
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                description.AppendSoftReturn();
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}