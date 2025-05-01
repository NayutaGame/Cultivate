
public class GainArmorDetails : StageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public StageClosureListener Initiator;
    public CastResult CastResult;
    public StageClosure[] Closures;

    public GainArmorDetails(StageEntity src, StageEntity tgt, int value, StageClosureListener initiator, CastResult castResult, StageClosure[] closures, bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Initiator = initiator;
        CastResult = castResult;
        Closures = closures;
        Induced = induced;
    }
    
    public GainArmorDetails ShallowClone() => new(Src, Tgt, Value, Initiator, CastResult, Closures, Induced);
}
