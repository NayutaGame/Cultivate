
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public abstract class ProcedureDefinition
{
    protected PreCondDefinition PreCondDefinition;
    protected PostCondDefinition PostCondDefinition;
    
    private List<StageClosure> _closures;
    private StageClosure[] _closureArray;
    public StageClosure[] Closures
    {
        get
        {
            if (_closureArray != null)
                return _closureArray;
            _closureArray = _closures.ToArray();
            return _closureArray;
        }
    }

    private Func<ProcedureDefinition, ResultDict, ResultDict, Description> _getDescription;
    
    public ProcedureDefinition()
    {
        PreCondDefinition = PreCondDefinition.Default;
        PostCondDefinition = PostCondDefinition.Default;
        _closures = new();
        _getDescription = DefaultGetDescription;
    }

    public ProcedureDefinition AddClosure(StageClosure closure)
    {
        _closures.Add(closure);
        _closureArray = null;
        return this;
    }

    public bool ContainsClosure(StageClosure closure)
        => _closures.Contains(closure);

    public async UniTask TryCast(CastDetails castDetails)
    {
        bool postCond = PostCondDefinition.GetCond(castDetails);
        if (postCond)
            await Cast(castDetails);
        
        castDetails.CastResult.Append(this, postCond);
    }
    
    public virtual async UniTask Cast(CastDetails castDetails)
    {
    }

    public Description GetDescription(ResultDict costResult, ResultDict castResult)
        => _getDescription(this, costResult, castResult);

    public ProcedureDefinition SetDescription(Func<ProcedureDefinition, ResultDict, ResultDict, Description> getDescription)
    {
        _getDescription = getDescription;
        return this;
    }

    public PostCondDefinition GetPostCondDefinition()
        => PostCondDefinition;

    public ProcedureDefinition SetPostCondDefinition(PostCondDefinition postCondDefinition)
    {
        PostCondDefinition = postCondDefinition;
        return this;
    }

    public PreCondDefinition GetPreCondDefinition()
        => PreCondDefinition;

    public ProcedureDefinition SetPreCondDefinition(PreCondDefinition preCondDefinition)
    {
        PreCondDefinition = preCondDefinition;
        return this;
    }

    public abstract Description DefaultGetDescription(ProcedureDefinition procedureDefinition, ResultDict costResult, ResultDict castResult);
}