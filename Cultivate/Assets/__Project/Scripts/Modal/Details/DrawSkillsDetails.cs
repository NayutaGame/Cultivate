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
}
