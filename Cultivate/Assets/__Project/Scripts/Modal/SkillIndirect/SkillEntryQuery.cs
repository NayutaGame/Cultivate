
using System;
using System.Collections.Generic;
using CLLibrary;

public sealed class SkillEntryQuery
{
    private List<Predicate<SkillEntry>> _predicates;
    private SkillEntry _entry;
    private WuXing _wuXing;
    private Bound _baseJingJieBound;
    private TagComposite _tagComposite;

    private SkillEntryQuery(
        List<Predicate<SkillEntry>> predicates = null,
        SkillEntry entry = null,
        WuXing wuXing = null,
        Bound? baseJingJieBound = null,
        TagComposite tagComposite = null)
    {
        _predicates = predicates ?? new List<Predicate<SkillEntry>>();
        _entry = entry;
        _wuXing = wuXing;
        _baseJingJieBound = baseJingJieBound ?? new(0, 4);
        _tagComposite = tagComposite;
    }

    public static SkillEntryQuery AnySkill()
        => new();

    public static SkillEntryQuery FromEntry(SkillEntry entry)
        => new(entry: entry);

    public static SkillEntryQuery FromId(string id)
        => new(entry: Encyclopedia.SkillCategory.FromId(id));

    public static SkillEntryQuery FromName(string name)
        => new(entry: Encyclopedia.SkillCategory.FromName(name));

    public static SkillEntryQuery FromBaseJingJieBound(Bound baseJingJieBound)
        => new(baseJingJieBound: baseJingJieBound);

    public static SkillEntryQuery FromBaseJingJieBoundTag(Bound baseJingJieBound, TagComposite tagComposite)
        => new(baseJingJieBound: baseJingJieBound, tagComposite: tagComposite);

    public static SkillEntryQuery FromPredicatesBaseJingJieBound(List<Predicate<SkillEntry>> predicates, Bound baseJingJieBound)
        => new(predicates, baseJingJieBound: baseJingJieBound);

    public static SkillEntryQuery FromWuXingBaseJingJieBound(WuXing wuXing, Bound baseJingJieBound)
        => new(wuXing: wuXing, baseJingJieBound: baseJingJieBound);

    public static SkillEntryQuery FromPredWuXingBaseJingJieBound(Predicate<SkillEntry> predicate, WuXing wuXing, Bound baseJingJieBound)
        => new(new() { predicate }, wuXing: wuXing, baseJingJieBound: baseJingJieBound);

    public static SkillEntryQuery FromSkillReference(SkillReference skillReference)
        => new(entry: skillReference.GetEntry());

    public static SkillEntryQuery FromEverything(
        SkillEntry entry = null,
        WuXing wuXing = null,
        Bound? baseJingJieBound = null,
        TagComposite tagComposite = null)
        => new(
            entry: entry,
            wuXing: wuXing,
            baseJingJieBound: baseJingJieBound,
            tagComposite: tagComposite);

    public static SkillEntryQuery FromEditorQuery(EditorSkillEntryQuery editorQuery)
        => new(
            entry: string.IsNullOrEmpty(editorQuery.EntryName) ? null : Encyclopedia.SkillCategory.FromName(editorQuery.EntryName),
            wuXing: (WuXingType.Any == editorQuery.WuXing) ? null : global::WuXing.FromWuXingType(editorQuery.WuXing),
            baseJingJieBound: editorQuery.BaseJingJieBound,
            tagComposite: TagComposite.FromTagType(editorQuery.Tag));

    public SkillEntryQuery Clone()
        => new(_predicates, _entry, _wuXing, _baseJingJieBound, _tagComposite);

    public List<SkillEntryQuery> Stack(int count)
    {
        List<SkillEntryQuery> toRet = new();
        for (int i = 0; i < count; i++)
            toRet.Add(Clone());
        return toRet;
    }
    
    public bool Matches(SkillEntry skillEntry)
    {
        if (_entry != null && skillEntry != _entry)
            return false;

        if (!_predicates.AllMatch(pred => pred(skillEntry)))
            return false;

        if (_wuXing != null && skillEntry.WuXing != _wuXing)
            return false;

        if (!_baseJingJieBound.Contains(skillEntry.LowestJingJie))
            return false;

        if (_tagComposite != null && !skillEntry.GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }
    
    public bool Matches(RunSkill skill)
    {
        if (_entry != null && skill.GetEntry() != _entry)
            return false;

        if (!_predicates.AllMatch(pred => pred(skill.GetEntry())))
            return false;

        if (_wuXing != null && skill.GetEntry().WuXing != _wuXing)
            return false;

        if (!_baseJingJieBound.Contains(skill.GetEntry().LowestJingJie))
            return false;

        if (_tagComposite != null && !skill.GetEntry().GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }
}