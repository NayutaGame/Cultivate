
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class SetActionPointProcedureDefinition : ProcedureDefinition
{
    public int ActionPoint;
    
    public SetActionPointProcedureDefinition(int actionPoint)
    {
        ActionPoint = actionPoint;
    }

    protected SetActionPointProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        int actionPoint) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        ActionPoint = actionPoint;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures[i] = Closures[i];

        return new SetActionPointProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            actionPoint: ActionPoint
        );
    }
    
    public override async UniTask Cast(CastDetails castDetails)
        => castDetails.Caster.SetActionPoint(ActionPoint);
    
    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        SetActionPointProcedureDefinition pd = procedureDefinition as SetActionPointProcedureDefinition;
        description.Sb.Append(pd.PostCondDefinition.Description);
        description.Sb.Append($"二动");
        // if (Closures != null)
        //     foreach (StageClosure c in Closures)
        //     {
        //         Description closureDescription = c.Description;
        //         // closureDescription.ApplyReplaceValues(castResult);
        //         // closureDescription.ApplyCastResult(castResult, c.Key);
        //         description.Sb.Append(closureDescription);
        //     }
        
        description.ApplyStyle(castResult, pd);
    }
}