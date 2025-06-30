
public class LoseArmorDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;

    public bool SpawnVFX;

    public LoseArmorDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        int value,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool induced,
        bool spawnVFX) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Listener = listener;
        Closures = closures;
        CastResult = castResult;
        SpawnVFX = spawnVFX;
    }
    
    public LoseArmorDetails ShallowClone() => new(Env, Src, Tgt, Value, Listener, Closures, CastResult, Induced, SpawnVFX);
}
