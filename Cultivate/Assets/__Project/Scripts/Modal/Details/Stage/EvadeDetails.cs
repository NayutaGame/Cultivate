using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    private int _value;
    public int Value
    {
        get => _value;
        set => _value = Mathf.Max(0, value);
    }

    public EvadeDetails(StageEntity src, StageEntity tgt, int value)
    {
        Src = src;
        Tgt = tgt;
        Value = value;
    }

    public EvadeDetails Clone() => new(Src, Tgt, _value);
}
