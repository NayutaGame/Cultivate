
public class GainMaxHealthDetails : NestedStageClosureDetails
{
    public StageEntity Entity;
    public int Value;

    public GainMaxHealthDetails(
        StageEnvironment env,
        StageEntity entity,
        int value,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Entity = entity;
        Value = value;
    }
    
    public GainMaxHealthDetails ShallowClone() => new(Env, Entity, Value, Listener, Closures, CastResult, ClosureHasRegistered, Induced);

    public static GainMaxHealthDetails FromHealPenetrate(HealDetails d, int gap)
        => new(d.Env, d.Tgt, gap, d.Listener, d.Closures, d.CastResult, d.ClosureHasRegistered, d.Induced);
}