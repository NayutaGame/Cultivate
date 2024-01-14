
using System;
using System.Collections.Generic;

public class DiscoverSkillDetails : EventDetails
{
    public List<RunSkill> Skills;
    public Predicate<SkillEntry> Pred;
    public WuXing? WuXing;
    public JingJie? JingJie;
    public int Count;

    public DiscoverSkillDetails(Predicate<SkillEntry> pred, WuXing? wuXing, JingJie? jingJie, int count)
    {
        Skills = new();
        Pred = pred;
        WuXing = wuXing;
        JingJie = jingJie;
        Count = count;
    }
}
