using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCost
{
    private Func<int, int[], int> _func;

    public ManaCost(Func<int, int[], int> func)
    {
        _func = func;
    }

    public int Eval(int level)
        => _func(level, new int[] { 0, 0, 0, 0, 0 });
    public int Eval(int level, int[] powers)
        => _func(level, powers);

    public static implicit operator ManaCost(int value) => new((level, powers) => value);
    public static implicit operator ManaCost(Func<int, int[], int> func) => new(func);
}
