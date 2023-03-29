using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipDescription
{
    public Func<int, JingJie, int[], string> _func;

    public ChipDescription(Func<int, JingJie, int[], string> func)
    {
        _func = func;
    }

    public string Eval(int level, JingJie jingJie, int[] powers)
        => _func(level, jingJie, powers);

    public static implicit operator ChipDescription(string s) => new((level, jingJie, powers) => s);
}
