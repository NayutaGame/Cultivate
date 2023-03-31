using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCost
{
    private Func<int, JingJie, int[], int> _func;

    public ManaCost(Func<int, JingJie, int[], int> func)
    {
        _func = func;
    }

    public int Eval(int level, JingJie jingJie)
        => _func(level, jingJie, new int[] { 0, 0, 0, 0, 0 });
    public int Eval(int level, JingJie jingJie, int[] powers)
        => _func(level, jingJie, powers);

    public static implicit operator ManaCost(int value) => new((level, jingJie, powers) => value);
}
