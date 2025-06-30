
public class LoseMaxHealthDetails : NestedStageClosureDetails
{
    public StageEntity Entity;
    public int Value;

    public LoseMaxHealthDetails(
        StageEnvironment env,
        StageEntity entity,
        int value,
        StageClosureListener listener,
        ResultDict castResult,
        StageClosure[] closures,
        bool induced) : base(env, induced)
    {
        Entity = entity;
        Value = value;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
    }
    
    public LoseMaxHealthDetails ShallowClone() => new(Env, Entity, Value, Listener, CastResult, Closures, Induced);
}