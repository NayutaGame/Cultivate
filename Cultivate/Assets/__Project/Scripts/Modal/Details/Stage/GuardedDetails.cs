using UnityEngine;

public class GuardedDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public GuardedDetails(StageEntity src, StageEntity tgt, int value)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }

    public GuardedDetails Clone() => new(Src, Tgt, _value);

    public static GuardedDetails FromAttackDetails(AttackDetails d)
        => new(d.Src, d.Tgt, d.Value);
}
