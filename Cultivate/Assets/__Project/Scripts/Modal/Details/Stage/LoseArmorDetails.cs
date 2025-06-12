
public class LoseArmorDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public bool SpawnVFX;

    public LoseArmorDetails(
        StageEntity src, StageEntity tgt, int value, StageClosureListener listener, StageClosure[] closures, ResultDict castResult, bool induced, bool spawnVFX)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Listener = listener;
        Closures = closures;
        CastResult = castResult;
        Induced = induced;
        SpawnVFX = spawnVFX;
    }
    
    public LoseArmorDetails ShallowClone() => new(Src, Tgt, Value, Listener, Closures, CastResult, Induced, SpawnVFX);
}
