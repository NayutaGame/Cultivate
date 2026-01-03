using System;
using System.Collections.Generic;
using System.Linq;
using CLLibrary;

public sealed class RunSkillQuery : AnnotatableLine
{
    private List<Predicate<RunSkill>> _predicates;
    private SkillEntry _entry;
    private WuXingPred _wuXingPred;
    private JingJiePred _jingJiePred;
    private Bound? _baseJingJieBound;
    private TagComposite _tagComposite;
    private Description _description;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    private RunSkillQuery(
        List<Predicate<RunSkill>> predicates = null,
        SkillEntry entry = null,
        WuXingPred wuXingPred = WuXingPred.任意,
        JingJiePred jingJiePred = JingJiePred.任意,
        Bound? baseJingJieBound = null,
        TagComposite tagComposite = null,
        Description description = null)
    {
        _predicates = predicates ?? new List<Predicate<RunSkill>>();
        _entry = entry;
        _wuXingPred = wuXingPred;
        _jingJiePred = jingJiePred;
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
        => new(entry: entry, jingJiePred: JingJie.ToPred(jingJie), description: new($"需要提交{entry.GetName()}卡牌"));

    public static RunSkillQuery FromNameJingJie(string name, JingJie jingJie)
        => new(entry: Encyclopedia.SkillCategory.FromName(name), jingJiePred: JingJie.ToPred(jingJie), description: new($"需要提交{name}卡牌"));

    public static RunSkillQuery FromWuXing(WuXing wuXing)
        => new(wuXingPred: WuXing.ToPred(wuXing), description: new($"请提交一张五行为{wuXing.GetName()}的牌"));

    public static RunSkillQuery FromSkillGhost(SkillGhost skillGhost)
        => new(entry: skillGhost.GetEntry(), jingJiePred: JingJie.ToPred(skillGhost.GetJingJie()));

    public static RunSkillQuery FromJingJieBound(int low, int high)
        => new(baseJingJieBound: new(low, high),
            description: new($"请提交一张境界在{((JingJie)low).GetName()}到{((JingJie)high).GetName()}之间的牌"));

    public static RunSkillQuery FromJingJieBoundAndHasWuXing(int low, int high)
        => new(baseJingJieBound: new(low, high), wuXingPred: WuXingPred.有五行,
            description: new($"请提交一张境界在{((JingJie)low).GetName()}到{((JingJie)high).GetName()}之间，具有五行的牌"));

    public static RunSkillQuery FromEditorQuery(EditorRunSkillQuery editorQuery)
    {
        JingJie lowBase = JingJie.FromIndirect(editorQuery.LowBaseJingJie);
        JingJie highBase = JingJie.FromIndirect(editorQuery.HighBaseJingJie);
        Bound baseJingjieBound = new(lowBase, highBase);

        List<Predicate<RunSkill>> predicates = new();
        
        if (editorQuery.AttackRequirement > 0)
            predicates.Add(AttackRequirementPred(editorQuery.AttackRequirement));

        if (editorQuery.ArmorRequirement > 0)
            predicates.Add(ArmorRequirementPred(editorQuery.ArmorRequirement));
        
        if (editorQuery.ManaRequirement > 0)
            predicates.Add(ManaRequirementPred(editorQuery.ManaRequirement));

        BuffEntry buffEntry = Encyclopedia.BuffCategory.FromName(editorQuery.BuffRequirement);
        if (buffEntry != null)
            predicates.Add(BuffRequirementPred(buffEntry));
        
        return new(
            predicates: predicates,
            entry: string.IsNullOrEmpty(editorQuery.EntryName) ? null : Encyclopedia.SkillCategory.FromName(editorQuery.EntryName),
            wuXingPred: editorQuery.WuXingPred,
            jingJiePred: editorQuery.JingJiePred,
            baseJingJieBound: baseJingjieBound,
            tagComposite: TagComposite.FromEditor(editorQuery.Tag),
            description: editorQuery.Description);
    }

    public RunSkillQuery Clone()
    {
        List<Predicate<RunSkill>> newList = new();
        foreach (Predicate<RunSkill> pred in _predicates)
            newList.Add(pred);
        return new RunSkillQuery(newList, _entry, _wuXingPred, _jingJiePred, _baseJingJieBound, _tagComposite, _description);
    }

    public List<RunSkillQuery> Stack(int stack)
    {
        List<RunSkillQuery> toRet = new();
        for (int i = 0; i < stack; i++)
            toRet.Add(Clone());
        return toRet;
    }
    
    public bool Matches(RunSkill runSkill)
    {
        if (!_predicates.AllMatch(pred => pred(runSkill)))
            return false;

        if (_entry != null && runSkill.GetEntry() != _entry)
            return false;

        if (!WuXing.PredIsMatch(_wuXingPred, runSkill.GetWuXing()))
            return false;

        if (!JingJie.PredIsMatch(_jingJiePred, runSkill.GetJingJie()))
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

    private static Predicate<RunSkill> AttackRequirementPred(int value)
    {
        return skill => skill.SkillDefinition.GetProcedureDefinitions()
            .Any(pd => pd is AttackProcedureDefinition attackPd && attackPd.Value >= value);
    }

    private static Predicate<RunSkill> ArmorRequirementPred(int value)
    {
        return skill => skill.SkillDefinition.GetProcedureDefinitions()
            .Any(pd => pd is GainArmorProcedureDefinition gainArmorPd && gainArmorPd.Value >= value);
    }

    private static Predicate<RunSkill> ManaRequirementPred(int value)
    {
        return skill => skill.SkillDefinition.GetProcedureDefinitions()
            .Any(pd => pd is GainBuffProcedureDefinition gainBuffPd && gainBuffPd.BuffEntry.GetName() == "灵气" && gainBuffPd.Stack >= value);
    }

    private static Predicate<RunSkill> BuffRequirementPred(BuffEntry buffEntry)
    {
        return skill => skill.SkillDefinition.GetProcedureDefinitions()
            .Any(pd => pd is GainBuffProcedureDefinition gainBuffPd && gainBuffPd.BuffEntry == buffEntry && gainBuffPd.Stack >= 1);
    }
}