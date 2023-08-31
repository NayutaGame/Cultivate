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
}
