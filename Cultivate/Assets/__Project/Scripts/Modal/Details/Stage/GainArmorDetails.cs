
public class GainArmorDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public GainArmorDetails(StageEntity src, StageEntity tgt, int value, StageClosureListener listener, CastResult castResult, StageClosure[] closures, bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
        Induced = induced;
    }
    
    public GainArmorDetails ShallowClone() => new(Src, Tgt, Value, Listener, CastResult, Closures, Induced);
}
