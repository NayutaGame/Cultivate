
public class CycleDetails : NestedStageClosureDetails
{
    public StageEntity Owner;
    public bool Rotate;
    public WuXing WuXing;
    public int Step;
    public int Gain;
    public int Recover;

    public int Flow;

    public CycleDetails(
        StageEnvironment env,
        StageEntity owner,
        bool rotate,
        WuXing wuXing,
        int gain,
        int recover,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Owner = owner;
        Rotate = rotate;
        WuXing = wuXing;
        Step = 1;
        Gain = gain;
        Recover = recover;
    }

    public CycleDetails ShallowClone() => new(Env, Owner, Rotate, WuXing, Gain, Recover, Listener, Closures, CastResult, ClosureHasRegistered, Induced);
}
