
using System;
using System.Collections.Generic;

public class SkillDescription
{
    public Func<JingJie, int, Dictionary<string, string>, string> _func;

    public SkillDescription(Func<JingJie, int, Dictionary<string, string>, string> func)
    {
        _func = func;
    }
    
    public string Get(JingJie jingJie, int dJingJie, Dictionary<string, string> indicator = null)
        => _func(jingJie, dJingJie, indicator);

    public static implicit operator SkillDescription(string s) => new((jingJie, dJingJie, indicator) => s);
}
