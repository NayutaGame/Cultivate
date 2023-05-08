using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class DrawSkillRewardDescriptor : RewardDescriptor
{
    private string _description;
    public Predicate<SkillEntry> _pred;
    private WuXing? _wuXing;
    private JingJie? _jingJie;
    public int _count;

    public DrawSkillRewardDescriptor(string description, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null, int count = 1)
    {
        _description = description;
        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _count = count;
    }

    public override void Claim()
    {
        bool success = RunManager.Instance.SkillPool.TryDrawSkills(out List<RunSkill> skills, _pred, _wuXing, _jingJie, _count);
        if (!success)
            throw new Exception();
        RunManager.Instance.Battle.SkillInventory.AddRange(skills);
    }

    public override string GetDescription() => _description;
}
