using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkillsDetails
{
    public Predicate<SkillEntry> _pred;
    public WuXing? _wuXing;
    public JingJie? _jingJie;
    public int _count;
    public bool _distinct;
    public bool _consume;

    public DrawSkillsDetails(Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null, int count = 1, bool distinct = true, bool consume = true)
    {
        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _count = count;
        _distinct = distinct;
        _consume = consume;
    }

    public bool CanDraw(SkillEntry skillEntry)
    {
        if (_pred != null && !_pred(skillEntry))
            return false;

        if (_wuXing != null && skillEntry.WuXing != _wuXing)
            return false;

        if (_jingJie != null && skillEntry.JingJieRange.Contains(_jingJie.Value))
            return false;

        return true;
    }
}
