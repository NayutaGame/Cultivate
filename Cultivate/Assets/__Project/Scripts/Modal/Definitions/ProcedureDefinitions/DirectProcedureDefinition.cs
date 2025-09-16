
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class DirectProcedureDefinition : ProcedureDefinition
{
    private Func<CastDetails, UniTask> _cast;
    
    public DirectProcedureDefinition(Func<CastDetails, UniTask> cast)
    {
        _cast = cast;
    }

    protected DirectProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures,
        Func<CastDetails, UniTask> cast) :
        base(preCondDefinition, postCondDefinition, getDescription, closures)
    {
        _cast = cast;
    }

    public override ProcedureDefinition Clone()
    {
        List<StageClosure> clonedClosures = new List<StageClosure>();
        for (int i = 0; i < Closures.Count; i++)
            clonedClosures.Add(Closures[i]);

        return new DirectProcedureDefinition(
            preCondDefinition: PreCondDefinition.Clone(),
            postCondDefinition: PostCondDefinition.Clone(),
            getDescription: _getDescription,
            closures: clonedClosures,
            cast: _cast
        );
    }
    
    public override async UniTask Cast(CastDetails castDetails)
        => await _cast(castDetails);
    
    public override void DefaultGetDescription(Description description, ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult)
    {
        DirectProcedureDefinition pd = procedureDefinition as DirectProcedureDefinition;
        description.Join(pd.PostCondDefinition.Description);
        
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