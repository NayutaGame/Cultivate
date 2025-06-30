
using UnityEngine;

public class GuardedDetails : StageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public GuardedDetails(StageEnvironment env, StageEntity src, StageEntity tgt, int value) : base(env)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }

    public GuardedDetails Clone() => new(Env, Src, Tgt, _value);

    public static GuardedDetails FromAttackDetails(AttackDetails d)
        => new(d.Env, d.Src, d.Tgt, d.Value);
}
