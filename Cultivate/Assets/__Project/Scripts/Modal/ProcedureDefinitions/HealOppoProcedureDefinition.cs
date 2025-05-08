
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class HealOppoProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Penetrate;
    public bool Induced;

    public HealOppoProcedureDefinition(int value,
        bool penetrate = false,
        bool induced = false)
    {
        Value = value;
        Penetrate = penetrate;
        Induced = induced;
    }

    protected HealOppoProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        int value,
        bool penetrate,
        bool induced) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        Value = value;
        Penetrate = penetrate;
        Induced = induced;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures[i] = Closures[i];

        return new HealOppoProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            value: Value,
            penetrate: Penetrate,
            induced: Induced
        );
    }
    
    public HealDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: Value,
            penetrate: Penetrate,
            listener: d.Skill,
            castResult: d.CastResult,
            closures: ClosuresArray,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.HealProcedure(GetDetailsFromCastDetails(castDetails));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        HealOppoProcedureDefinition pd = procedureDefinition as HealOppoProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        description.Sb.Append($"敌方气血+{pd.Value}");
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                // closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}