
public class HealDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Penetrate;

    public HealDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        bool penetrate,
        StageClosureListener listener,
        ResultDict castResult,
        StageClosure[] closures,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Penetrate = penetrate;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
        Induced = induced;
    }

    public HealDetails ShallowClone() => new(Src, Tgt, Value, Penetrate, Listener, CastResult, Closures, Induced);
}
