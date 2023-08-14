using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelTimeEvaluator
{
    private Func<JingJie, int, bool, int> _func;

    public ChannelTimeEvaluator(Func<JingJie, int, bool, int> func)
    {
        _func = func;
    }

    public int Eval(JingJie jingJie, int dJingJie, bool jiaShi)
        => _func(jingJie, dJingJie, jiaShi);

    public static implicit operator ChannelTimeEvaluator(int value) => new((jingJie, dJingJie, jiaShi) => value);
}
