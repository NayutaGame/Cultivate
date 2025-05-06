
public class LoseArmorDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public LoseArmorDetails(StageEntity src, StageEntity tgt, int value, StageClosureListener listener, StageClosure[] closures, ResultDict castResult, bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Listener = listener;
        Closures = closures;
        CastResult = castResult;
        Induced = induced;
    }
    
    public LoseArmorDetails Clone() => new(Src, Tgt, Value, Listener, Closures, CastResult, Induced);
}
