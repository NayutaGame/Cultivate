using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiGongType
{
    public static readonly WaiGongType None = 0x00000000;
    public static readonly WaiGongType Attack = 0x00000001;
    public static readonly WaiGongType JianZhen = 0x00000010;

    private int _value;

    public bool Contains(WaiGongType other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public static implicit operator int(WaiGongType type) => type._value;
    public static implicit operator WaiGongType(int value) => new() { _value = value };
}
