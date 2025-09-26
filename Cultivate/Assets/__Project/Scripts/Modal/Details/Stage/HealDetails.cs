
public class HealDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public int Value;
    public bool Penetrate;

    public HealDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        int value,
        bool penetrate,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Penetrate = penetrate;
    }

    public HealDetails ShallowClone() => new(Env, Src, Tgt, Value, Penetrate, Listener, Closures, CastResult, ClosureHasRegistered, Induced);

    public static HealDetails FromLifeSteal(DamageDetails d)
        => new(d.Env, d.Src, d.Src, d.Value, false, d.Listener, d.Closures, d.CastResult, d.ClosureHasRegistered, true);
}
