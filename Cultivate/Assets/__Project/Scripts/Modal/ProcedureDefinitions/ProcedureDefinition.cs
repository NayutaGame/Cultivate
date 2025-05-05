
using System;
using Cysharp.Threading.Tasks;

public abstract class ProcedureDefinition
{
    protected PreCondDefinition PreCondDefinition;
    protected PostCondDefinition PostCondDefinition;
    private Func<ProcedureDefinition, CostResult, CastResult, Description> _getDescription;
    
    public ProcedureDefinition()
    {
        PreCondDefinition = PreCondDefinition.Default;
        PostCondDefinition = PostCondDefinition.Default;
        _getDescription = DefaultGetDescription;
    }

    public async UniTask TryCast(StageEnvironment env, CastDetails castDetails)
    {
        bool postCond = PostCondDefinition.Cond(env, castDetails);
        if (postCond)
            await Cast(env, castDetails);
        
        castDetails.CastResult.Append(this, postCond);
    }
    
    public virtual async UniTask Cast(StageEnvironment env, CastDetails castDetails)
    {
    }

    public Description GetDescription(CostResult costResult, CastResult castResult)
        => _getDescription(this, costResult, castResult);

    public ProcedureDefinition SetDescription(Func<ProcedureDefinition, CostResult, CastResult, Description> getDescription)
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

    public abstract Description DefaultGetDescription(ProcedureDefinition procedureDefinition, CostResult costResult, CastResult castResult);
}