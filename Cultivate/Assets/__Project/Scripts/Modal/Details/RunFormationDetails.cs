
using System.Collections.Generic;
using UnityEngine;

public class RunFormationDetails : EventDetails
{
    public RunEntity Owner;

    public int[] WuXingCounts;
    public List<WuXing> WuXingOrder;

    public int SwiftCount;
    public int NonSwiftCount;

    public int ExhaustedCount;
    public int NonExhaustedCount;

    public int AttackCount;
    public int NonAttackCount;

    public int TotalCostCount;
    public int HighestConsecutiveAttackCount;

    public int Proficiency;

    public RunFormationDetails(RunEntity entity)
    {
        Owner = entity;

        WuXingCounts = new int[WuXing.Length];
        SwiftCount = 0;
        ExhaustedCount = 0;
        AttackCount = 0;
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

                AttackCount++;
            }
            else
            {
                consecutiveAttackCount = 0;
            }

            TotalCostCount += entry.GetCostDescription(jingJie).ByType(CostDescription.CostType.Mana);
        }

        WuXingOrder = new List<WuXing>(WuXing.Traversal);
        WuXingOrder.Sort((lhs, rhs) => WuXingCounts[rhs] - WuXingCounts[lhs]);

        NonSwiftCount = entity.GetSlotLimit() - SwiftCount;
        NonExhaustedCount = entity.GetSlotLimit() - ExhaustedCount;
        NonAttackCount = entity.GetSlotLimit() - AttackCount;

        Proficiency = 0;
    }
}
