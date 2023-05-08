using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCost
{
    private Func<JingJie, int, int> _func;

    public ManaCost(Func<JingJie, int, int> func)
    {
        _func = func;
    }

    public int Eval(JingJie jingJie, int dJingJie)
        => _func(jingJie, dJingJie);

    public static implicit operator ManaCost(int value) => new((jingJie, dJingJie) => value);
}
