
using System;
using CLLibrary;

public class RunSkillDescriptor
{
    private Predicate<RunSkill> _pred;
    
    private WuXing _wuXing;
    private Bound? _jingJieBound;
    private TagComposite _tagComposite;

    public RunSkillDescriptor(
        Predicate<RunSkill> pred = null,
        WuXing wuXing = null,
        Bound? jingJieBound = null,
        TagComposite tagComposite = null)
    {
        _pred = pred;
        _wuXing = wuXing;
        _jingJieBound = jingJieBound;
        _tagComposite = tagComposite;
    }
    
    public bool Contains(RunSkill runSkill)
    {
        if (_pred != null && !_pred(runSkill))
            return false;

        if (_wuXing != null && runSkill.GetWuXing() != _wuXing)
            return false;

        if (_jingJieBound != null && !_jingJieBound.Value.Contains(runSkill.JingJie))
            return false;

        if (_tagComposite != null && !runSkill.GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }

    public static RunSkillDescriptor FromJingJieBound(JingJie low, JingJie highExclusive)
        => new(jingJieBound: new Bound(low, highExclusive));

    public static RunSkillDescriptor FromTagComposite(TagComposite tagComposite)
        => new(tagComposite: tagComposite);
}
