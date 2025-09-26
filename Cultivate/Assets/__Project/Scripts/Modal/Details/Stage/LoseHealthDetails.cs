
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
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Victim = victim;
        Value = value;
        CausedByAttack = causedByAttack;
    }

    public static LoseHealthDetails FromDamageDetails(DamageDetails d)
        => new(d.Env, d.Tgt, d.Value, d.CausedByAttack, d.Listener, d.Closures, d.CastResult, d.ClosureHasRegistered, d.Induced);
}
