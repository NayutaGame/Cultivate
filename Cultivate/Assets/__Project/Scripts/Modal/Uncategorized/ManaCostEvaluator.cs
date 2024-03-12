
using System;

public class ManaCostEvaluator
{
    private Func<JingJie, int, bool, int> _func;

    public ManaCostEvaluator(Func<JingJie, int, bool, int> func)
    {
        _func = func;
    }

    public int Eval(JingJie jingJie, int dJingJie, bool jiaShi)
        => _func(jingJie, dJingJie, jiaShi);

    public static implicit operator ManaCostEvaluator(int value) => new((jingJie, dJingJie, jiaShi) => value);
}
