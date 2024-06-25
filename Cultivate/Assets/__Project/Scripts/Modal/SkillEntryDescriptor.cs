
using System;
using CLLibrary;
using UnityEngine;

public class SkillEntryDescriptor : ISkillModel
{
    private Predicate<SkillEntry> _pred;
    
    private SkillEntry _entry;
    public SkillEntry Entry => _entry;
    private WuXing? _wuXing;
    private JingJie? _jingJie;
    public JingJie? JingJie => _jingJie;
    private SkillTypeComposite _skillTypeComposite;

    public SkillEntryDescriptor(
        Predicate<SkillEntry> pred = null,
        SkillEntry entry = null,
        WuXing? wuXing = null,
        JingJie? jingJie = null,
        SkillTypeComposite skillTypeComposite = null)
    {
        _pred = pred;
        _entry = entry;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _skillTypeComposite = skillTypeComposite;
    }

    public static SkillEntryDescriptor FromRunSkill(RunSkill runSkill)
        => new(entry: runSkill.GetEntry(), jingJie: runSkill.JingJie);

    public static SkillEntryDescriptor FromEntry(SkillEntry entry)
        => new(entry: entry);

    public static SkillEntryDescriptor FromPredJingJie(Predicate<SkillEntry> pred, JingJie? jingJie)
        => new(pred: pred, jingJie: jingJie);
    
    public static SkillEntryDescriptor FromJingJie(JingJie jingJie)
        => new(jingJie: jingJie);

    public static SkillEntryDescriptor FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry: entry, jingJie: jingJie);

    public static SkillEntryDescriptor FromWuXingJingJie(WuXing wuXing, JingJie jingJie)
        => new(wuXing: wuXing, jingJie: jingJie);
    
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

        if (_skillTypeComposite != null && !skillEntry.GetSkillTypeComposite().Contains(_skillTypeComposite))
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

    public Sprite GetWuXingSprite()
        => _entry?.GetWuXingSprite();

    public string GetName()
        => _entry?.GetName();

    public SkillTypeComposite GetSkillTypeComposite()
        => _entry?.GetSkillTypeComposite();

    public string GetExplanation()
        => _entry?.GetExplanation();

    public string GetTrivia()
        => _entry?.GetTrivia();

    public JingJie GetJingJie()
        => _jingJie ?? global::JingJie.LianQi;

    public CostDescription GetCostDescription(JingJie showingJingJie)
        => _entry?.GetCostDescription(showingJingJie) ?? new CostDescription();

    public string GetHighlight(JingJie showingJingJie)
        => _entry?.GetHighlight(showingJingJie);

    public Sprite GetJingJieSprite(JingJie showingJingJie)
        => _entry?.GetJingJieSprite(showingJingJie);

    public JingJie NextJingJie(JingJie showingJingJie)
        => _entry?.NextJingJie(showingJingJie) ?? global::JingJie.LianQi;

    public Color GetColor()
        => _entry?.GetColor() ?? Color.white;
}
