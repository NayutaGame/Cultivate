
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class LoseArmorProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Induced;

    public LoseArmorProcedureDefinition(int value,
        bool induced = false)
    {
        Value = value;
        Induced = induced;
    }

    protected LoseArmorProcedureDefinition(
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

        return new LoseArmorProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            value: Value,
            induced: Induced
        );
    }

    public LoseArmorDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            env: d.Env,
            src: d.Caster,
            tgt: d.Caster,
            value: Value,
            listener: d.Skill,
            closures: ClosuresArray,
            castResult: d.CastResult,
            induced: Induced,
            spawnVFX: true);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.LoseArmorProcedure(GetDetailsFromCastDetails(castDetails));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        LoseArmorProcedureDefinition pd = procedureDefinition as LoseArmorProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        
        if (pd.Value > 0)
            description.Sb.Append($"失去{pd.Value}护甲");
        
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