
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

    public override async UniTask Cast(CastDetails castDetails)
        => await castDetails.Env.ExhaustProcedure(castDetails.Caster, castDetails.Skill);

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        ExhaustProcedureDefinition pd = procedureDefinition as ExhaustProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        description.Sb.Append($"升华");
        // if (Closures != null)
        //     foreach (StageClosure c in Closures)
        //     {
        //         Description closureDescription = c.Description;
        //         closureDescription.ApplyReplaceValues(castResult);
        //         // closureDescription.ApplyCastResult(castResult, c.Key);
        //         description.Sb.Append(closureDescription);
        //     }
        
        description.ApplyStyle(castResult, pd);
    }
}