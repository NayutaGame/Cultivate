
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class GainMaxHealthProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Induced;

    public GainMaxHealthProcedureDefinition(int value, bool induced)
    {
        Value = value;
        Induced = induced;
    }

    protected GainMaxHealthProcedureDefinition(
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

        return new GainMaxHealthProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            value: Value,
            induced: Induced
        );
    }
    
    public GainMaxHealthDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            entity: d.Caster,
            value: Value,
            listener: d.Skill,
            castResult: d.CastResult,
            closures: ClosuresArray,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.GainMaxHealthProcedure(GetDetailsFromCastDetails(castDetails));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        GainMaxHealthProcedureDefinition pd = procedureDefinition as GainMaxHealthProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        
        if (pd.Value != 0)
            description.Sb.Append($"气血上限+{pd.Value}");
        
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