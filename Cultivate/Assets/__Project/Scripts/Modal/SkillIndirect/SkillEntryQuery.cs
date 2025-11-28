
using System;
using System.Collections.Generic;
using CLLibrary;

public sealed class SkillEntryQuery
{
    private List<Predicate<SkillEntry>> _predicates;
    private SkillEntry _entry;
    private WuXingPred _wuXingPred;
    private Bound _baseJingJieBound;
    private TagComposite _tagComposite;

    private SkillEntryQuery(
        List<Predicate<SkillEntry>> predicates = null,
        SkillEntry entry = null,
        WuXingPred wuXingPred = WuXingPred.任意,
        Bound? baseJingJieBound = null,
        TagComposite tagComposite = null)
    {
        _predicates = predicates ?? new List<Predicate<SkillEntry>>();
        _entry = entry;
        _wuXingPred = wuXingPred;
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
        => new(wuXingPred: WuXing.ToPred(wuXing), baseJingJieBound: baseJingJieBound);

    public static SkillEntryQuery FromPredWuXingBaseJingJieBound(Predicate<SkillEntry> predicate, WuXing wuXing, Bound baseJingJieBound)
        => new(new() { predicate }, wuXingPred: WuXing.ToPred(wuXing), baseJingJieBound: baseJingJieBound);

    public static SkillEntryQuery FromSkillGhost(SkillGhost skillGhost)
        => new(entry: skillGhost.GetEntry());

    public static SkillEntryQuery FromEditorQuery(EditorSkillEntryQuery editorQuery)
    {
        JingJie lowBase = JingJie.FromIndirect(editorQuery.LowBaseJingJie);
        JingJie highBase = JingJie.FromIndirect(editorQuery.HighBaseJingJie);
        Bound baseJingjieBound = new(lowBase, highBase);
        return new SkillEntryQuery(
            entry: string.IsNullOrEmpty(editorQuery.EntryName) ? null : Encyclopedia.SkillCategory.FromName(editorQuery.EntryName),
            wuXingPred: editorQuery.WuXingPred,
            baseJingJieBound: baseJingjieBound,
            tagComposite: TagComposite.FromEditor(editorQuery.Tag));
    }

    public static List<SkillEntryQuery> FromEditorQueries(List<EditorSkillEntryQuery> editorQueries, JingJie preferredJingJie)
    {
        if (editorQueries == null || editorQueries.Count <= 0)
            return FromBaseJingJieBound(new(JingJie.LianQi, preferredJingJie)).Stack(3);
        
        List<SkillEntryQuery> toRet = new List<SkillEntryQuery>(editorQueries.Count);
        for (int i = 0; i < editorQueries.Count; i++)
        {
            var eq = editorQueries[i];
            if (eq == null) continue;
            toRet.Add(FromEditorQuery(eq));
        }
        return toRet;
    }

    public SkillEntryQuery Clone()
        => new(_predicates, _entry, _wuXingPred, _baseJingJieBound, _tagComposite);

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

        if (!WuXing.PredIsMatch(_wuXingPred, skillEntry.WuXing))
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

        if (!WuXing.PredIsMatch(_wuXingPred, skill.GetEntry().WuXing))
            return false;

        if (!_baseJingJieBound.Contains(skill.GetEntry().LowestJingJie))
            return false;

        if (_tagComposite != null && !skill.GetEntry().GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }
}