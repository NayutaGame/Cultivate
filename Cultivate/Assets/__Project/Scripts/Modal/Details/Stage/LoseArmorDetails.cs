
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
        bool closureHasRegistered,
        bool induced,
        bool spawnVFX) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        SpawnVFX = spawnVFX;
    }
    
    public LoseArmorDetails ShallowClone() => new(Env, Src, Tgt, Value, Listener, Closures, CastResult, ClosureHasRegistered, Induced, SpawnVFX);

    public static LoseArmorDetails FromAttackDetails(AttackDetails d, int negate)
        => new(d.Env, d.Src, d.Tgt, negate, d.Listener, d.Closures, d.CastResult, d.ClosureHasRegistered, true, false);
    
    public static LoseArmorDetails FromIndirectDetails(IndirectDetails d, int negate)
        => new(d.Env, d.Src, d.Tgt, negate, d.SrcSkill, null, d.CastResult, false, true, false);
}
