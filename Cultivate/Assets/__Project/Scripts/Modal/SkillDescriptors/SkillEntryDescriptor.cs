
using System;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class SkillEntryDescriptor : AnnotatableSkill
{
    private Predicate<SkillEntry> _pred;
    
    private SkillEntry _entry;
    public SkillEntry Entry => _entry;
    private WuXing _wuXing;
    private JingJie _jingJie;
    public JingJie JingJie => _jingJie;
    private TagComposite _tagComposite;
    
    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        // { "TagComposite",               thisObject => ((AnnotatableSkill)thisObject).GetTagComposite() },
    };
    public object Get(string s) => Accessor[s](this);
    public SkillEntryDescriptor(
        Predicate<SkillEntry> pred = null,
        SkillEntry entry = null,
        WuXing wuXing = null,
        JingJie jingJie = null,
        TagComposite tagComposite = null)
    {
        _pred = pred;
        _entry = entry;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _tagComposite = tagComposite;
    }

    public static SkillEntryDescriptor FromRunSkill(RunSkill runSkill)
        => new(entry: runSkill.GetEntry(), jingJie: runSkill.JingJie);

    public static SkillEntryDescriptor FromEntry(SkillEntry entry)
        => new(entry: entry);

    public static SkillEntryDescriptor FromId(string id)
        => new(entry: Encyclopedia.SkillCategory.FromId(id));

    public static SkillEntryDescriptor FromName(string name)
        => new(entry: Encyclopedia.SkillCategory.FromName(name));

    public static SkillEntryDescriptor FromNameJingJie(string name, JingJie jingJie)
        => new(entry: Encyclopedia.SkillCategory.FromName(name), jingJie: jingJie);

    public static SkillEntryDescriptor FromPredJingJie(Predicate<SkillEntry> pred, JingJie jingJie)
        => new(pred: pred, jingJie: jingJie);
    
    public static SkillEntryDescriptor FromJingJie(JingJie jingJie)
        => new(jingJie: jingJie);

    public static SkillEntryDescriptor FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry: entry, jingJie: jingJie);

    public static SkillEntryDescriptor FromWuXingJingJie(WuXing wuXing, JingJie jingJie)
        => new(wuXing: wuXing, jingJie: jingJie);

    public static SkillEntryDescriptor AnySkill()
        => new();
    
    public bool Contains(SkillEntry skillEntry)
    {
        if (_entry != null && skillEntry != _entry)
            return false;
        
        if (_pred != null && !_pred(skillEntry))
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
        
        if (_pred != null && !_pred(skill.GetEntry()))
            return false;

        if (_wuXing != null && skill.GetEntry().WuXing != _wuXing)
            return false;

        if (_jingJie != null && skill.JingJie != _jingJie)
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


    public JingJie GetLowestJingJie()
        => _entry?.GetLowestJingJie() ?? _jingJie;

    public JingJie GetHighestJingJie()
        => _entry?.GetHighestJingJie() ?? _jingJie;

    public Sprite GetSprite()
        => _entry?.GetSprite();

    public WuXing GetWuXing()
        => _entry?.WuXing;

    public string GetName()
        => _entry?.GetName();

    public Description GetDescription(JingJie showingJingJie)
        => _entry?.GetDescription(showingJingJie) ?? "";

    public TagComposite GetTagComposite()
        => _entry?.GetTagComposite();

    public string GetTrivia()
        => _entry?.GetTrivia();

    public JingJie GetJingJie()
        => _jingJie ?? JingJie.LianQi;

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => _entry?.GetLiteralCostDescription(showingJingJie) ?? CostDescription.Empty;

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => _entry?.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => _entry?.NextJingJie(showingJingJie) ?? JingJie.LianQi;

    public bool CanShowAnnotation()
        => _entry != null;
}
