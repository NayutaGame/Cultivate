
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class HealProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Penetrate;
    public bool Induced;

    public HealProcedureDefinition(int value,
        bool penetrate = false,
        bool induced = false)
    {
        Value = value;
        Penetrate = penetrate;
        Induced = induced;
    }

    protected HealProcedureDefinition(
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
            clonedClosures.Add(Closures[i]);

        return new HealProcedureDefinition(
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
            env: d.Env,
            src: d.Caster,
            tgt: d.Caster,
            value: Value,
            penetrate: Penetrate,
            listener: d.Skill,
            closures: ClosuresArray,
            castResult: d.CastResult,
            closureHasRegistered: false,
            induced: Induced);

    public override async UniTask Cast(CastDetails d)
        => await d.Env.HealProcedure(GetDetailsFromCastDetails(d));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        HealProcedureDefinition pd = procedureDefinition as HealProcedureDefinition;
        description.Join(pd.PostCondDefinition.Description);
        if (pd.Value > 0)
            description.Join($"气血+{pd.Value}");
        
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                description.AppendSoftReturn();
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyResult(castResult, c.Key);
                description.Join(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}