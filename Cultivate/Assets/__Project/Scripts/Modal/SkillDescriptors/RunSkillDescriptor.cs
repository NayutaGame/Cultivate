
using System;
using CLLibrary;

public class RunSkillDescriptor
{
    private Predicate<RunSkill> _pred;
    
    private WuXing? _wuXing;
    private Bound? _jingJieBound;
    private SkillTypeComposite _skillTypeComposite;

    public RunSkillDescriptor(
        Predicate<RunSkill> pred = null,
        WuXing? wuXing = null,
        Bound? jingJieBound = null,
        SkillTypeComposite skillTypeComposite = null)
    {
        _pred = pred;
        _wuXing = wuXing;
        _jingJieBound = jingJieBound;
        _skillTypeComposite = skillTypeComposite;
    }
    
    public bool Contains(RunSkill runSkill)
    {
        if (_pred != null && !_pred(runSkill))
            return false;

        if (_wuXing != null && runSkill.GetWuXing() != _wuXing)
            return false;

        if (_jingJieBound != null && !_jingJieBound.Value.Contains(runSkill.JingJie))
            return false;

        if (_skillTypeComposite != null && !runSkill.GetSkillTypeComposite().Contains(_skillTypeComposite))
            return false;

        return true;
    }

    public static RunSkillDescriptor FromJingJieBound(JingJie low, JingJie highExclusive)
        => new(jingJieBound: new Bound(low, highExclusive));

    public static RunSkillDescriptor FromSkillTypeComposite(SkillTypeComposite skillTypeComposite)
        => new(skillTypeComposite: skillTypeComposite);
}
