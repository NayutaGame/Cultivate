
using System;

public class SkillDescription
{
    public Func<JingJie, int, ResultDict, string> _func;

    public SkillDescription(Func<JingJie, int, ResultDict, string> func)
    {
        _func = func;
    }
    
    public string Get(JingJie jingJie, int dJingJie, ResultDict castResult = null)
        => _func(jingJie, dJingJie, castResult);

    public static implicit operator SkillDescription(string s) => new((jingJie, dJingJie, executeResult) => s);
}
