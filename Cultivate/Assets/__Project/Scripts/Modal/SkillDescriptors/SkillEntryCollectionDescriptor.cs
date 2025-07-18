
using System;

public class SkillEntryCollectionDescriptor
{
    private Predicate<SkillEntry> _pred;
    
    private WuXing? _wuXing;
    private JingJie? _jingJie;
    public JingJie? JingJie => _jingJie;
    private TagComposite _tagComposite;
    private int _count;
    public int Count => _count;
    private bool _distinct;
    public bool Distinct => _distinct;
    private bool _consume;
    public bool Consume => _consume;

    public SkillEntryCollectionDescriptor(
        Predicate<SkillEntry> pred = null,
        WuXing? wuXing = null,
        JingJie? jingJie = null,
        TagComposite tagComposite = null,
        int count = 1,
        bool distinct = true,
        bool consume = true)
    {
        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _tagComposite = tagComposite;
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

        if (_tagComposite != null && !skillEntry.GetTagComposite().Contains(_tagComposite))
            return false;

        return true;
    }
}
