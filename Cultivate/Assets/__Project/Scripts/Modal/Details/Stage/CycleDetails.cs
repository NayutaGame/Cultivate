
public class CycleDetails : StageClosureDetails
{
    public StageEntity Owner;
    public bool Rotate;
    public WuXing WuXing;
    public int Step;
    public int Gain;
    public int Recover;
    public StageClosureListener Initiator;
    public StageClosure[] Closures;
    public CastResult CastResult;

    public int Flow;

    public CycleDetails(
        StageEntity owner,
        bool rotate,
        WuXing wuXing,
        int gain,
        int recover,
        StageClosureListener initiator,
        StageClosure[] closures,
        CastResult castResult,
        bool induced)
    {
        Owner = owner;
        Rotate = rotate;
        WuXing = wuXing;
        Step = 1;
        Gain = gain;
        Recover = recover;
        Initiator = initiator;
        Closures = closures;
        CastResult = castResult;
        Induced = induced;
    }

    public CycleDetails ShallowClone() => new(Owner, Rotate, WuXing, Gain, Recover, Initiator, Closures, CastResult, Induced);
}
