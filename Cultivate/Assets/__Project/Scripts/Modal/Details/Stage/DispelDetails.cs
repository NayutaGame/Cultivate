
public class DispelDetails : NestedStageClosureDetails
{
    public StageEntity Entity;
    public int Value;

    public DispelDetails(
        StageEnvironment env,
        StageEntity entity,
        int value,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool induced) : base(env, induced)
    {
        Entity = entity;
        Value = value;
        Listener = listener;
        Closures = closures;
        CastResult = castResult;
    }
}
