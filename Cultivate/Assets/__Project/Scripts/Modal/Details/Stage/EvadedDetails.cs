
using UnityEngine;

public class EvadedDetails : StageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public EvadedDetails(StageEnvironment env, StageEntity src, StageEntity tgt, int value, bool induced) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }

    public EvadedDetails Clone() => new(Env, Src, Tgt, _value, Induced);

    public static EvadedDetails FromAttackDetails(AttackDetails d)
        => new(d.Env, d.Src, d.Tgt, d.Value, d.Induced);
}
