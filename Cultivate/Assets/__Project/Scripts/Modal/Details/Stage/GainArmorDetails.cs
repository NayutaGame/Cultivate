
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
        ResultDict castResult,
        StageClosure[] closures,
        bool induced) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
    }
    
    public GainArmorDetails ShallowClone() => new(Env, Src, Tgt, Value, Listener, CastResult, Closures, Induced);
}
