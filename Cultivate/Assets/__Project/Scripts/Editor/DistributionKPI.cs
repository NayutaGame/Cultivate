
using System.Collections.Generic;

public class DistributionKPI
{
    public static readonly Dictionary<WuXing, (int Min, int Max)> ManaCostTargets = new()
    {
        { WuXing.Jin, (0, 1) },    // 正常消耗
        { WuXing.Shui, (1, 3) },   // 大量消耗
        { WuXing.Mu, (1, 3) },     // 大量消耗
        { WuXing.Huo, (0, 1) },    // 不消耗
        { WuXing.Tu, (0, 0) }      // 少量消耗
    };

    public static readonly Dictionary<WuXing, (int Min, int Max)> HealthCostTargets = new()
    {
        { WuXing.Jin, (0, 0) },    // 不消耗
        { WuXing.Shui, (0, 0) },   // 不消耗
        { WuXing.Mu, (0, 0) },     // 不消耗
        { WuXing.Huo, (1, 3) },    // 大量消耗
        { WuXing.Tu, (0, 1) }      // 少量消耗
    };

    public static readonly Dictionary<WuXing, (int Min, int Max)> ChannelCostTargets = new()
    {
        { WuXing.Jin, (0, 1) },    // 少量消耗
        { WuXing.Shui, (0, 0) },   // 不消耗
        { WuXing.Mu, (0, 1) },     // 不消耗
        { WuXing.Huo, (0, 2) },    // 正常消耗
        { WuXing.Tu, (1, 3) }      // 大量消耗
    };

    private static readonly Dictionary<JingJie, float> JingJieWeights = new()
    {
        { JingJie.LianQi, 1.0f },   // 练气期最重要
        { JingJie.ZhuJi, 1.0f },    // 筑基期同样重要
        { JingJie.JinDan, 0.8f },   // 金丹期次之
        { JingJie.YuanYing, 0.6f }, // 元婴期影响较小
    };

    public static float CalculateScore(Dictionary<SkillDistributionKey, List<SkillEntry>> distribution)
    {
        float totalScore = 0;
        float totalWeight = 0;
        
        foreach (var jingJie in JingJie.Traversal)
        {
            float weight = JingJieWeights.GetValueOrDefault(jingJie, 0f);

            foreach (var wuXing in WuXing.Traversal)
            {
                var key = new SkillDistributionKey(jingJie, wuXing);
                int count = distribution.GetValueOrDefault(key)?.Count ?? 0;

                // 检查每种消耗类型的分布
                float manaScore = EvaluateDistribution(count, ManaCostTargets[wuXing]);
                float healthScore = EvaluateDistribution(count, HealthCostTargets[wuXing]);
                float channelScore = EvaluateDistribution(count, ChannelCostTargets[wuXing]);

                float cellScore = (manaScore + healthScore + channelScore) / 3;
                totalScore += cellScore * weight;
                totalWeight += weight;
            }
        }

        return totalScore / totalWeight * 100; // 转换为百分比
    }

    private static float EvaluateDistribution(int count, (int Min, int Max) target)
    {
        if (count < target.Min)
            return count / (float)target.Min;
        if (count > target.Max)
            return target.Max / (float)count;
        return 1.0f;
    }
}
