
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class GainArmorProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Induced;

    public GainArmorProcedureDefinition(int value,
        bool induced = false)
    {
        Value = value;
        Induced = induced;
    }

    protected GainArmorProcedureDefinition(
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

        return new GainArmorProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            value: Value,
            induced: Induced
        );
    }
    
    public GainArmorDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            env: d.Env,
            src: d.Caster,
            tgt: d.Caster,
            value: Value,
            listener: d.Skill,
            castResult: d.CastResult,
            closures: ClosuresArray,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.GainArmorProcedure(GetDetailsFromCastDetails(castDetails));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        GainArmorProcedureDefinition pd = procedureDefinition as GainArmorProcedureDefinition;
        description.Join(pd.PostCondDefinition.Description);
        
        if (pd.Value != 0)
            description.Join($"护甲+{pd.Value}");
        
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