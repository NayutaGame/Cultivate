
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
            clonedClosures.Add(Closures[i]);

        return new SetActionPointProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            actionPoint: ActionPoint
        );
    }
    
    public override async UniTask Cast(CastDetails d)
        => d.Caster.SetActionPoint(ActionPoint);
    
    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        SetActionPointProcedureDefinition pd = procedureDefinition as SetActionPointProcedureDefinition;
        description.Join(pd.PostCondDefinition.Description);
        
        if (ActionPoint == 2)
            description.Join($"二动");
        else if (ActionPoint == 3)
            description.Join($"三动");
        else if (ActionPoint == 4)
            description.Join($"四动");
        
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