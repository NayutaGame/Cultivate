using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDescription
{
    public Func<JingJie, int, string> _func;

    public SkillDescription(Func<JingJie, int, string> func)
    {
        _func = func;
    }

    public string Eval(JingJie jingJie, int dJingJie)
        => _func(jingJie, dJingJie);

    public static implicit operator SkillDescription(string s) => new((jingJie, dJingJie) => s);
}
