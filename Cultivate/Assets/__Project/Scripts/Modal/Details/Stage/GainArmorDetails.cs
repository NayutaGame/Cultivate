
public class GainArmorDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public GainArmorDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        int value,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }
    
    public GainArmorDetails ShallowClone() => new(Env, Src, Tgt, Value, Listener, Closures, CastResult, ClosureHasRegistered, Induced);
}
