
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class RemoveHealthProcedureDefinition : ProcedureDefinition
{
    public int Value;
    public bool Induced;

    public RemoveHealthProcedureDefinition(int value,
        bool induced = false)
    {
        Value = value;
        Induced = induced;
    }

    protected RemoveHealthProcedureDefinition(
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

        return new RemoveHealthProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            value: Value,
            induced: Induced
        );
    }

    public LoseHealthDetails GetDetailsFromCastDetails(CastDetails d)
        => new(
            env: d.Env,
            victim: d.Caster.Opponent(),
            value: Value,
            causedByAttack: false,
            listener: d.Skill,
            closures: ClosuresArray,
            castResult: d.CastResult,
            induced: Induced);

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.LoseHealthProcedure(GetDetailsFromCastDetails(castDetails));

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        RemoveHealthProcedureDefinition pd = procedureDefinition as RemoveHealthProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        description.Sb.Append($"对手失去{pd.Value}气血");
        
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