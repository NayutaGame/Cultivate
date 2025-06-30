
public class LoseHealthDetails : NestedStageClosureDetails
{
    public StageEntity Victim;
    public int Value;
    public bool CausedByAttack;

    public LoseHealthDetails(
        StageEnvironment env,
        StageEntity victim,
        int value,
        bool causedByAttack,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool induced) : base(env, induced)
    {
        Victim = victim;
        Value = value;
        CausedByAttack = causedByAttack;
        Listener = listener;
        Closures = closures;
        CastResult = castResult;
    }
}
