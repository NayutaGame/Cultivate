
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ExhaustProcedureDefinition : ProcedureDefinition
{
    public ExhaustProcedureDefinition() { }
    
    protected ExhaustProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new ExhaustProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures
        );
    }

    public override async UniTask Cast(CastDetails d)
        => await d.Env.ExhaustProcedure(d.Caster, d.Skill);

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        ExhaustProcedureDefinition pd = procedureDefinition as ExhaustProcedureDefinition;
        description.Join(pd.PostCondDefinition.Description);
        description.Join($"升华");
        
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