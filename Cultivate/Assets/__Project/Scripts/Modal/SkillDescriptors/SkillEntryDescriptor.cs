
using System;
using CLLibrary;
using UnityEngine;

public class SkillEntryDescriptor : ISkill
{
    private Predicate<SkillEntry> _pred;
    
    private SkillEntry _entry;
    public SkillEntry Entry => _entry;
    private WuXing? _wuXing;
    private JingJie? _jingJie;
    public JingJie? JingJie => _jingJie;
    private TagComposite _tagComposite;

    public SkillEntryDescriptor(
        Predicate<SkillEntry> pred = null,
        SkillEntry entry = null,
        WuXing? wuXing = null,
        JingJie? jingJie = null,
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

    public static SkillEntryDescriptor FromName(string name)
        => new(entry: SkillEntry.FromName(name));

    public static SkillEntryDescriptor FromNameJingJie(string name, JingJie jingJie)
        => new(entry: SkillEntry.FromName(name), jingJie: jingJie);

    public static SkillEntryDescriptor FromPredJingJie(Predicate<SkillEntry> pred, JingJie? jingJie)
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

        if (_jingJie != null && !skillEntry.JingJieContains(_jingJie.Value))
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
    
    public static implicit operator SkillEntryDescriptor(string id) => FromEntry(id);
    public static implicit operator SkillEntryDescriptor(SkillEntry skillEntry) => FromEntry(skillEntry);





    public int GetCurrCounter() => 0;
    public int GetMaxCounter() => 0;

    public Sprite GetSprite()
        => _entry?.GetSprite();

    public WuXing? GetWuXing()
        => _entry?.WuXing;

    public string GetName()
        => _entry?.GetName();

    public TagComposite GetTagComposite()
        => _entry?.GetTagComposite();

    public string GetCascadeAnnotated()
        => _entry?.GetCascadeAnnotated();

    public string GetTrivia()
        => _entry?.GetTrivia();

    public JingJie GetJingJie()
        => _jingJie ?? global::JingJie.LianQi;

    public CostDescription GetLiteralCostDescription(JingJie showingJingJie)
        => _entry?.GetLiteralCostDescription(showingJingJie) ?? CostDescription.Empty;

    public string GetHighlight(JingJie showingJingJie)
        => _entry?.GetHighlight(showingJingJie);

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => _entry?.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => _entry?.NextJingJie(showingJingJie) ?? global::JingJie.LianQi;
}
