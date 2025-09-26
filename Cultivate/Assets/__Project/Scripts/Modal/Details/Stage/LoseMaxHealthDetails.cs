
public class LoseMaxHealthDetails : NestedStageClosureDetails
{
    public StageEntity Entity;
    public int Value;

    public LoseMaxHealthDetails(
        StageEnvironment env,
        StageEntity entity,
        int value,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Entity = entity;
        Value = value;
    }
    
    public LoseMaxHealthDetails ShallowClone() => new(Env, Entity, Value, Listener, Closures, CastResult, ClosureHasRegistered, Induced);
}