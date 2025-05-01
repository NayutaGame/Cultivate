
public class HealDetails : StageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Penetrate;
    public StageClosureListener Initiator;
    public CastResult CastResult;
    public StageClosure[] Closures;

    public HealDetails(
        StageEntity src,
        StageEntity tgt,
        int value,
        bool penetrate,
        StageClosureListener initiator,
        CastResult castResult,
        StageClosure[] closures,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Penetrate = penetrate;
        Initiator = initiator;
        CastResult = castResult;
        Closures = closures;
        Induced = induced;
    }

    public HealDetails ShallowClone() => new(Src, Tgt, Value, Penetrate, Initiator, CastResult, Closures, Induced);
}
