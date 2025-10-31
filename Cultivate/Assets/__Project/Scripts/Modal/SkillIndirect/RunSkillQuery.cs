using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public sealed class RunSkillQuery : AnnotatableLine
{
    private Predicate<RunSkill> _pred;
    private SkillEntry _entry;
    private WuXing _wuXing;
    private JingJie _jingJie;
    private Bound? _baseJingJieBound;
    private TagComposite _tagComposite;
    private Description _description;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    private RunSkillQuery(
        Predicate<RunSkill> pred = null,
        SkillEntry entry = null,
        WuXing wuXing = null,
        JingJie jingJie = null,
        Bound? baseJingJieBound = null,
        TagComposite tagComposite = null,
        Description description = null)
    {
        _pred = pred;
        _entry = entry;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _baseJingJieBound = baseJingJieBound;
        _tagComposite = tagComposite;
        _description = description ?? new("未设定的描述");
    }

    public static RunSkillQuery AnySkill()
        => new(description: new("请提交任意一张牌"));

    public static RunSkillQuery FromName(string name)
        => new(entry: Encyclopedia.SkillCategory.FromName(name), description: new($"需要提交{name}卡牌"));

    public static RunSkillQuery FromId(string id)
        => new(entry: Encyclopedia.SkillCategory.FromId(id), description: new($"需要提交{Encyclopedia.SkillCategory.FromId(id).GetName()}卡牌"));

    public static RunSkillQuery FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry: entry, jingJie: jingJie, description: new($"需要提交{entry.GetName()}卡牌"));

    public static RunSkillQuery FromNameJingJie(string name, JingJie jingJie)
        => new(entry: Encyclopedia.SkillCategory.FromName(name), jingJie: jingJie, description: new($"需要提交{name}卡牌"));

    public static RunSkillQuery FromWuXing(WuXing wuXing)
        => new(wuXing: wuXing, description: new($"请提交一张五行为{wuXing.GetName()}的牌"));

    public static RunSkillQuery FromSkillReference(SkillReference skillReference)
        => new(entry: skillReference.GetEntry(), jingJie: skillReference.GetJingJie());

    public static RunSkillQuery FromJingJieBound(int low, int high)
        => new(baseJingJieBound: new(low, high),
            description: new($"请提交一张境界在{((JingJie)low).GetName()}到{((JingJie)high).GetName()}之间的牌"));

    public static RunSkillQuery FromJingJieBoundAndHasWuXing(int low, int high)
        => new(baseJingJieBound: new(low, high), pred: s => s.GetWuXing() != WuXing.Wu,
            description: new($"请提交一张境界在{((JingJie)low).GetName()}到{((JingJie)high).GetName()}之间，具有五行的牌"));

    public static RunSkillQuery FromTagComposite(TagComposite tagComposite)
        => new(tagComposite: tagComposite, description: new($"请提交一张包含{tagComposite.GetTagListString()}的牌"));

    public static RunSkillQuery FromWuXingJingJieTagComposite(WuXing wuXing, JingJie jingJie, TagComposite tagComposite)
        => new(wuXing: wuXing, baseJingJieBound: new((int)jingJie, (int)jingJie), tagComposite: tagComposite, 
            description: new($"请提交一张五行为{wuXing.GetName()}，境界为{jingJie.GetName()}，包含{tagComposite.GetTagListString()}的牌"));

    public static RunSkillQuery FromEditorQuery(EditorRunSkillQuery editorQuery)
        => new(
            entry: string.IsNullOrEmpty(editorQuery.EntryName) ? null : Encyclopedia.SkillCategory.FromName(editorQuery.EntryName),
            wuXing: WuXing.FromEditor(editorQuery.WuXing),
            jingJie: JingJie.FromEditor(editorQuery.JingJie),
            baseJingJieBound: editorQuery.BaseJingJieBound,
            tagComposite: TagComposite.FromEditor(editorQuery.Tag),
            description: editorQuery.Description);

    public RunSkillQuery Clone()
        => new(_pred, _entry, _wuXing, _jingJie, _baseJingJieBound, _tagComposite, _description);
    
    public List<RunSkillQuery> Stack(int stack)
    {
        List<RunSkillQuery> toRet = new();
        for (int i = 0; i < stack; i++)
            toRet.Add(Clone());
        return toRet;
    }
    
    public bool Matches(RunSkill runSkill)
    {
        if (_pred != null && !_pred(runSkill))
            return false;

        if (_entry != null && runSkill.GetEntry() != _entry)
            return false;

        if (_wuXing != null && runSkill.GetWuXing() != _wuXing)
            return false;

        if (_jingJie != null && runSkill.GetJingJie() != _jingJie)
            return false;

        if (_baseJingJieBound != null && !_baseJingJieBound.Value.Contains(runSkill.GetJingJie()))
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