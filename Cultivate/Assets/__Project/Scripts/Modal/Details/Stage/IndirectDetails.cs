
using UnityEngine;

public class IndirectDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public WuXing? WuXing;
    public bool Recursive;

    public IndirectDetails(StageEntity src, StageEntity tgt, int value,
        WuXing? wuxing,
        bool recursive = true)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
        WuXing = wuxing;
        Recursive = recursive;
    }

    public IndirectDetails Clone() => new(Src, Tgt, _value, WuXing, Recursive);
}
