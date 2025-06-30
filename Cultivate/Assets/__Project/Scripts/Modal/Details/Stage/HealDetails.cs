
public class HealDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Penetrate;

    public HealDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        int value,
        bool penetrate,
        StageClosureListener listener,
        ResultDict castResult,
        StageClosure[] closures,
        bool induced) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Penetrate = penetrate;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
    }

    public HealDetails ShallowClone() => new(Env, Src, Tgt, Value, Penetrate, Listener, CastResult, Closures, Induced);
}
