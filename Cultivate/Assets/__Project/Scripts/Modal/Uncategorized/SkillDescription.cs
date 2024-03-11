
using System;

public class SkillDescription
{
    public Func<JingJie, int, ExecuteResult, string> _func;

    public SkillDescription(Func<JingJie, int, ExecuteResult, string> func)
    {
        _func = func;
    }
    
    public string Get(JingJie jingJie, int dJingJie, ExecuteResult executeResult = null)
        => _func(jingJie, dJingJie, executeResult);

    public static implicit operator SkillDescription(string s) => new((jingJie, dJingJie, executeResult) => s);
}
