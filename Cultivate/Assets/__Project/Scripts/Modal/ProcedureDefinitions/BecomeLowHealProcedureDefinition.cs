
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class BecomeLowHealProcedureDefinition : ProcedureDefinition
{
    public BecomeLowHealProcedureDefinition() { }

    protected BecomeLowHealProcedureDefinition(
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

        return new BecomeLowHealProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures
        );
    }

    public override async UniTask Cast(CastDetails castDetails)
    {
        if (Closures != null)
            foreach (StageClosure closure in Closures)
            {
                if (closure.Description == null)
                    return;
                castDetails.CastResult.Append(closure.Key, false);
            }
        await castDetails.BecomeLowHealth();
    }

    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        BecomeLowHealProcedureDefinition pd = procedureDefinition as BecomeLowHealProcedureDefinition;
        
        description.Sb.Append(pd.PostCondDefinition.Description);
        description.Sb.Append("成为残血");
        
        if (pd.Closures != null)
            foreach (StageClosure c in pd.Closures)
            {
                Description closureDescription = c.Description;
                closureDescription.ApplyReplaceValues(castResult);
                closureDescription.ApplyCastResult(castResult, c.Key);
                description.Sb.Append(closureDescription);
            }
        
        description.ApplyStyle(castResult, pd);
    }
}