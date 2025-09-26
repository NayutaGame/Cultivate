
using UnityEngine;

public class EvadedDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    private EvadedDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        int value,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced) : base(env, listener, closures, castResult, closureHasRegistered, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }

    public EvadedDetails Clone() => new(Env, Src, Tgt, _value, Listener, Closures, CastResult, ClosureHasRegistered, Induced);

    public static EvadedDetails FromAttackDetails(AttackDetails d)
        => new(d.Env, d.Src, d.Tgt, d.Value, d.Listener, d.Closures, d.CastResult, d.ClosureHasRegistered, d.Induced);
}
