
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
            clonedClosures.Add(Closures[i]);

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
            env: d.Env,
            src: d.Caster,
            tgt: d.Caster.Opponent(),
            value: Value,
            penetrate: Penetrate,
            fromLifesteal: false,
            listener: d.Skill,
            closures: ClosuresArray,
            castResult: d.CastResult,
            closureHasRegistered: false,
            induced: Induced);

    public override async UniTask Cast(CastDetails d)
        => await d.Env.HealProcedure(GetDetailsFromCastDetails(d));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        HealOppoProcedureDefinition pd = procedureDefinition as HealOppoProcedureDefinition;
        description.Join(pd.PostCondDefinition.Description);
        description.Join($"敌方气血+{pd.Value}");
        
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