using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WaiGongType
{
    public static readonly WaiGongType None =     0x00000000;
    public static readonly WaiGongType Attack =   0x00000001;
    public static readonly WaiGongType JianZhen = 0x00000010;
    public static readonly WaiGongType LingQi =   0x00000100;

    private int _value;
    public int Value => _value;

    public bool Contains(WaiGongType other)
    {
        return ((this & other) == other) &&
               (this | other) == this;
    }

    public static implicit operator int(WaiGongType type) => type._value;
    public static implicit operator WaiGongType(int value) => new() { _value = value };

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        if (Contains(Attack))
            sb.Append("攻击 ");

        if (Contains(JianZhen))
            sb.Append("剑阵 ");

        if (Contains(LingQi))
            sb.Append("灵气 ");

        return sb.ToString();
    }
}
