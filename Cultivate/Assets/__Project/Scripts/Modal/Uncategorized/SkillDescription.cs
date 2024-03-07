
using System;
using System.Collections.Generic;

public class SkillDescription
{
    public Func<JingJie, int, Dictionary<string, object>, string> _func;

    public SkillDescription(Func<JingJie, int, Dictionary<string, object>, string> func)
    {
        _func = func;
    }

    public string FromJDJ(JingJie jingJie, int dJingJie)
        => _func(jingJie, dJingJie, null);

    public string FromIndicator(JingJie jingJie, int dJingJie, Dictionary<string, object> indicator)
        => _func(jingJie, dJingJie, indicator);

    public static implicit operator SkillDescription(string s) => new((jingJie, dJingJie, indicator) => s);
}
