
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
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Entity = entity;
        Value = value;
    }
}
