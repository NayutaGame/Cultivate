
using UnityEngine;

public class EvadedDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public EvadedDetails(StageEntity src, StageEntity tgt, int value)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }

    public EvadedDetails Clone() => new(Src, Tgt, _value);
}
