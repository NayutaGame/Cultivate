
using System;
using System.Collections.Generic;
using CLLibrary;

public class RunSkillDescriptor : AnnotatableLine
{
    private Predicate<RunSkill> _pred;
    private WuXing _wuXing;
    private Bound? _jingJieBound;
    private TagComposite _tagComposite;
    private Description _description;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    private RunSkillDescriptor(
        Predicate<RunSkill> pred = null,
        WuXing wuXing = null,
        Bound? jingJieBound = null,
        TagComposite tagComposite = null,
        Description description = null)
    {
        _pred = pred;
        _wuXing = wuXing;
        _jingJieBound = jingJieBound;
        _tagComposite = tagComposite;
        _description = description ?? new("未设定的描述");
    }

    public static RunSkillDescriptor AnySkill()
        => new(description: new("请提交任意一张牌"));

    public static RunSkillDescriptor FromWuXing(WuXing wuXing)
        => new(wuXing: wuXing, description: new($"请提交一张五行为{wuXing.GetName()}的牌"));

    public static RunSkillDescriptor FromJingJieBound(JingJie low, JingJie highExclusive)
        => new(jingJieBound: new Bound(low, highExclusive),
            description: new($"请提交一张境界在{((JingJie)low).GetName()}到{((JingJie)(highExclusive - 1)).GetName()}之间的牌"));

    public static RunSkillDescriptor FromTagComposite(TagComposite tagComposite)
        => new(tagComposite: tagComposite, description: new($"请提交一张包含{tagComposite.GetTagListString()}的牌"));

    public RunSkillDescriptor Clone()
        => new(_pred, _wuXing, _jingJieBound, _tagComposite, _description);
    
    public bool Contains(RunSkill runSkill)
    {
        if (_pred != null && !_pred(runSkill))
            return false;

        if (_wuXing != null && runSkill.GetWuXing() != _wuXing)
            return false;

        if (_jingJieBound != null && !_jingJieBound.Value.Contains(runSkill.GetJingJie()))
            return false;

        if (_tagComposite != null && !runSkill.GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }

    public bool CanShowAnnotation()
        => true;

    public Description GetDescription()
        => _description;
}
