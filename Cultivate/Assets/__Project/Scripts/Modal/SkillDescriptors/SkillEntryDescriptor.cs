
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class SkillEntryDescriptor : AnnotatableSkill
{
    private List<Predicate<SkillEntry>> _predicates;
    private SkillEntry _entry;
    private WuXing _wuXing;
    private JingJie _jingJie;
    private TagComposite _tagComposite;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
        { "PackEntry",                  thisObject => ((AnnotatableSkill)thisObject).GetPackEntry() },
    };
    public object Get(string s) => Accessor[s](this);
    private SkillEntryDescriptor(
        List<Predicate<SkillEntry>> predicates = null,
        SkillEntry entry = null,
        WuXing wuXing = null,
        JingJie jingJie = null,
        TagComposite tagComposite = null)
    {
        _predicates = predicates ?? new List<Predicate<SkillEntry>>();
        _entry = entry;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _tagComposite = tagComposite;
    }

    public static SkillEntryDescriptor FromRunSkill(RunSkill runSkill)
        => new(entry: runSkill.GetEntry(), jingJie: runSkill.GetJingJie());

    public static SkillEntryDescriptor FromEntry(SkillEntry entry)
        => new(entry: entry);

    public static SkillEntryDescriptor FromId(string id)
    {
        SkillEntry entry = Encyclopedia.SkillCategory.FromId(id);
        if (entry == null)
            Debug.Log("Undesirable Behaviour");
        return new(entry: entry);
    }

    public static SkillEntryDescriptor FromName(string name)
        => new(entry: Encyclopedia.SkillCategory.FromName(name));

    public static SkillEntryDescriptor FromNameJingJie(string name, JingJie jingJie)
        => new(entry: Encyclopedia.SkillCategory.FromName(name), jingJie: jingJie);

    public static SkillEntryDescriptor FromPredJingJie(List<Predicate<SkillEntry>> predicates, JingJie jingJie)
        => new(predicates: predicates, jingJie: jingJie);
    
    public static SkillEntryDescriptor FromJingJie(JingJie jingJie)
        => new(jingJie: jingJie);

    public static SkillEntryDescriptor FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry: entry, jingJie: Mathf.Clamp(jingJie, entry.LowestJingJie, entry.HighestJingJie));

    public static SkillEntryDescriptor FromWuXingJingJie(WuXing wuXing, JingJie jingJie)
        => new(wuXing: wuXing, jingJie: jingJie);

    public static SkillEntryDescriptor FromPredWuXingJingJie(Predicate<SkillEntry> pred, WuXing wuXing, JingJie jingJie)
        => new(predicates: pred == null ? null : new List<Predicate<SkillEntry>> { pred }, wuXing: wuXing, jingJie: jingJie);

    public static SkillEntryDescriptor AnySkill()
        => new();
    
    public SkillEntry Entry => _entry;
    
    public JingJie JingJie => _jingJie;

    public JingJie GetLowestJingJie()
        => _entry?.GetLowestJingJie() ?? _jingJie;

    public JingJie GetHighestJingJie()
        => _entry?.GetHighestJingJie() ?? _jingJie;

    public Sprite GetCardIllustration() => _entry?.GetCardIllustration();
    public Sprite GetBarIllustration() => _entry?.GetBarIllustration();

    public string GetName()
        => _entry?.GetName();

    public Description GetDescription(JingJie showingJingJie)
        => _entry?.GetDescription(showingJingJie) ?? "";

    public string GetTrivia()
        => _entry.GetTrivia();

    public TagComposite GetTagComposite()
        => _entry?.GetTagComposite();

    public PackEntry GetPackEntry()
        => _entry?.GetPackEntry();

    public JingJie GetJingJie()
        => _jingJie ?? JingJie.LianQi;

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => _entry?.GetLiteralCostDescription(showingJingJie) ?? CostDescription.Empty;

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => _entry?.GetJingJieSprite(showingJingJie);

    public bool CanShowAnnotation()
        => _entry != null;
    
    public bool Contains(SkillEntry skillEntry)
    {
        if (_entry != null && skillEntry != _entry)
            return false;

        if (!_predicates.AllMatch(pred => pred(skillEntry)))
            return false;

        if (_wuXing != null && skillEntry.WuXing != _wuXing)
            return false;

        if (_jingJie != null && !skillEntry.JingJieContains(_jingJie))
            return false;

        if (_tagComposite != null && !skillEntry.GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }
    
    public bool Contains(RunSkill skill)
    {
        if (_entry != null && skill.GetEntry() != _entry)
            return false;

        if (!_predicates.AllMatch(pred => pred(skill.GetEntry())))
            return false;

        if (_wuXing != null && skill.GetEntry().WuXing != _wuXing)
            return false;

        if (_jingJie != null && skill.GetJingJie() != _jingJie)
            return false;

        if (_tagComposite != null && !skill.GetEntry().GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }
    
    public bool Contains(SkillEntryDescriptor descriptor)
    {
        if (_entry != null && _entry != descriptor._entry)
            return false;

        return descriptor.Contains(_entry);
    }
    
    public static implicit operator SkillEntryDescriptor(SkillEntry skillEntry) => FromEntry(skillEntry);
}
