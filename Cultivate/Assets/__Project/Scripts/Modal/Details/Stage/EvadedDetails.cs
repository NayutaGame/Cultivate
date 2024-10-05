
using UnityEngine;

public class EvadedDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public EvadedDetails(StageEntity src, StageEntity tgt, int value, bool induced)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        Induced = induced;
    }

    public EvadedDetails Clone() => new(Src, Tgt, _value, Induced);

    public static EvadedDetails FromAttackDetails(AttackDetails d)
        => new(d.Src, d.Tgt, d.Value, d.Induced);
}
