
using System;

public class DrawSkillReward : Reward
{
    private string _description;
    public Predicate<SkillEntry> _pred;
    private WuXing? _wuXing;
    private JingJie? _jingJie;
    public int _count;

    public DrawSkillReward(string description, Predicate<SkillEntry> pred = null, WuXing? wuXing = null, JingJie? jingJie = null, int count = 1)
    {
        _description = description;
        _pred = pred;
        _wuXing = wuXing;
        _jingJie = jingJie;
        _count = count;
    }

    public override void Claim()
        => RunManager.Instance.Environment.ForceDrawSkills(_pred, _wuXing, _jingJie, _count);

    public override string GetDescription() => _description;
}
