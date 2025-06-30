
public class GainMaxHealthDetails : NestedStageClosureDetails
{
    public StageEntity Entity;
    public int Value;

    public GainMaxHealthDetails(
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
    
    public GainMaxHealthDetails ShallowClone() => new(Env, Entity, Value, Listener, CastResult, Closures, Induced);
}