using System;

public struct SkillDistributionKey
{
    public JingJie JingJie;
    public WuXing? WuXing;

    public SkillDistributionKey(JingJie jingJie, WuXing? wuXing)
    {
        JingJie = jingJie;
        WuXing = wuXing;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is SkillDistributionKey other))
            return false;
            
        return JingJie == other.JingJie && WuXing == other.WuXing;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(JingJie, WuXing);
    }
}
