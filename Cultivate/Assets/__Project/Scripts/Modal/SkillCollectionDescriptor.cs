
using System;

public class SkillCollectionDescriptor
{
    private Predicate<SkillEntry> _pred;
    
    private WuXing? _wuXing;
    private JingJie? _jingJie;
    public JingJie? JingJie => _jingJie;
    private SkillTypeComposite _skillTypeComposite;
    private int _count;
    public int Count => _count;
    private bool _distinct;
    public bool Distinct => _distinct;
    private bool _consume;
    public bool Consume => _consume;

    public SkillCollectionDescriptor(
        Predicate<SkillEntry> pred = null,
        WuXing? wuXing = null,
        JingJie? jingJie = null,
        SkillTypeComposite skillTypeComposite = null,
        int count = 1,
        bool distinct = true,
        bool consume = true)
    {
        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _skillTypeComposite = skillTypeComposite;
        _count = count;
        _distinct = distinct;
        _consume = consume;
    }
    
    public bool Pred(SkillEntry skillEntry)
    {
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
}
