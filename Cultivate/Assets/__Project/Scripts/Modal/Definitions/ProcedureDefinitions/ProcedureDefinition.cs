
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public abstract class ProcedureDefinition
{
    public PreCondDefinition PreCondDefinition;
    public PostCondDefinition PostCondDefinition;
    
    private List<StageClosure> _closures;
    public List<StageClosure> Closures => _closures;
    private StageClosure[] _closureArray;
    public StageClosure[] ClosuresArray
    {
        get
        {
            if (_closureArray != null)
                return _closureArray;
            _closureArray = _closures.ToArray();
            return _closureArray;
        }
    }

    protected Action<Description, ProcedureDefinition, ResultDict, ResultDict> _getDescription;
    
    public ProcedureDefinition()
    {
        PreCondDefinition = PreCondDefinition.Default;
        PostCondDefinition = PostCondDefinition.Default;
        _closures = new();
        _getDescription = DefaultGetDescription;
    }

    protected ProcedureDefinition(
        PreCondDefinition preCondDefinition,
        PostCondDefinition postCondDefinition,
        Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription,
        List<StageClosure> closures)
    {
        PreCondDefinition = preCondDefinition;
        PostCondDefinition = postCondDefinition;
        _getDescription = getDescription;
        _closures = closures;
    }

    public abstract ProcedureDefinition Clone();

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
        bool postCond = await PostCondDefinition.GetCond(castDetails);
        if (postCond)
            await Cast(castDetails);
        
        castDetails.CastResult.Append(this, postCond);
    }
    
    public virtual async UniTask Cast(CastDetails d)
    {
    }

    public void GetDescription(Description description, ResultDict costResult, ResultDict castResult)
    {
        _getDescription(description, this, costResult, castResult);
    }

    public ProcedureDefinition SetDescription(Action<Description, ProcedureDefinition, ResultDict, ResultDict> getDescription)
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

    public abstract void DefaultGetDescription(
        Description description,
        ProcedureDefinition procedureDefinition,
        ResultDict costResult,
        ResultDict castResult);
}