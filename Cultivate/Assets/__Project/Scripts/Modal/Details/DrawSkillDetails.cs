using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSkillDetails
{
    public Predicate<SkillEntry> _pred;
    public WuXing? _wuXing;
    public JingJie? _jingJie;

    public DrawSkillDetails(Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null)
    {
        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie;
    }

    public bool CanDraw(SkillEntry skillEntry)
    {
        if (_pred != null && !_pred(skillEntry))
            return false;

        if (_wuXing != null && skillEntry.WuXing != _wuXing)
            return false;

        if (_jingJie != null && !skillEntry.JingJieContains(_jingJie.Value))
            return false;

        return true;
    }
}
