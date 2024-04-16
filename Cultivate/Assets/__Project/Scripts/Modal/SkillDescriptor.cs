
using System;

public class SkillDescriptor
{
    private Predicate<SkillEntry> _pred;
    
    private SkillEntry _entry;
    public SkillEntry Entry => _entry;
    private WuXing? _wuXing;
    private JingJie? _jingJie;
    public JingJie? JingJie => _jingJie;
    private SkillTypeComposite _skillTypeComposite;

    public SkillDescriptor(
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

    public static SkillDescriptor FromRunSkill(RunSkill runSkill)
        => new(entry: runSkill.GetEntry(), jingJie: runSkill.JingJie);

    public static SkillDescriptor FromEntry(SkillEntry entry)
        => new(entry: entry);

    public static SkillDescriptor FromPredJingJie(Predicate<SkillEntry> pred, JingJie? jingJie)
        => new(pred: pred, jingJie: jingJie);
    
    public static SkillDescriptor FromJingJie(JingJie jingJie)
        => new(jingJie: jingJie);

    public static SkillDescriptor FromEntryJingJie(SkillEntry entry, JingJie jingJie)
        => new(entry: entry, jingJie: jingJie);

    public static SkillDescriptor FromWuXingJingJie(WuXing wuXing, JingJie jingJie)
        => new(wuXing: wuXing, jingJie: jingJie);
    
    public bool Pred(SkillEntry skillEntry)
    {
        if (_entry != null && skillEntry != _entry)
            return false;
        
        if (_pred != null && !_pred(skillEntry))
            return false;

        if (_wuXing != null && skillEntry.WuXing != _wuXing)
            return false;

        if (_jingJie != null && !skillEntry.JingJieContains(_jingJie.Value))
            return false;

        if (_skillTypeComposite != null && !skillEntry.SkillTypeComposite.Contains(_skillTypeComposite))
            return false;

        return true;
    }
    
    public static implicit operator SkillDescriptor(string id) => FromEntry(id);
    public static implicit operator SkillDescriptor(SkillEntry skillEntry) => FromEntry(skillEntry);
}
