
using System.Collections.Generic;
using UnityEngine;

public class RunFormationDetails : EventDetails
{
    public int[] WuXingCounts;
    public List<WuXing> WuXingOrder;
    public int SwiftCount;
    public int ExhaustedCount;
    public int NonAttackCount;
    public int TotalCostCount;
    public int HighestConsecutiveAttackCount;

    public RunFormationDetails(RunEntity entity)
    {
        WuXingCounts = new int[WuXing.Length];
        SwiftCount = 0;
        ExhaustedCount = 0;
        NonAttackCount = 0;
        TotalCostCount = 0;
        HighestConsecutiveAttackCount = 0;

        int consecutiveAttackCount = 0;

        foreach (SkillSlot slot in entity.TraversalCurrentSlots())
        {
            SkillEntry entry = slot.PlacedSkill.Entry;
            JingJie jingJie = slot.PlacedSkill.JingJie;

            WuXing? wuXing = entry.WuXing;
            if (wuXing != null)
                WuXingCounts[wuXing.Value]++;

            if (entry.SkillTypeComposite.Contains(SkillType.ErDong))
                SwiftCount++;

            if (entry.SkillTypeComposite.Contains(SkillType.XiaoHao))
                ExhaustedCount++;

            if (entry.SkillTypeComposite.Contains(SkillType.Attack))
            {
                consecutiveAttackCount++;
                HighestConsecutiveAttackCount = Mathf.Max(HighestConsecutiveAttackCount, consecutiveAttackCount);
            }
            else
            {
                consecutiveAttackCount = 0;
                NonAttackCount++;
            }

            TotalCostCount += entry.GetBaseManaCost(jingJie);
        }

        WuXingOrder = new List<WuXing>(WuXing.Traversal);
        WuXingOrder.Sort((lhs, rhs) => WuXingCounts[rhs] - WuXingCounts[lhs]);
    }
}
