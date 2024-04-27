
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
        NonSwiftCount = 0;
        ExhaustedCount = 0;
        NonExhaustedCount = 0;
        AttackCount = 0;
        NonAttackCount = 0;
        TotalCostCount = 0;
        HighestConsecutiveAttackCount = 0;

        int consecutiveAttackCount = 0;

        foreach (SkillSlot slot in entity.TraversalCurrentSlots())
        {
            SkillEntry entry = slot.PlacedSkill.Entry;
            JingJie jingJie = slot.PlacedSkill.JingJie;

            if (slot.Skill?.GetEntry() == null)
            {
                consecutiveAttackCount = 0;
                continue;
            }

            WuXing? wuXing = entry.WuXing;
            if (wuXing != null)
                WuXingCounts[wuXing.Value]++;

            if (entry.GetSkillTypeComposite().Contains(SkillType.ErDong))
                SwiftCount++;
            else
                NonSwiftCount++;

            if (entry.GetSkillTypeComposite().Contains(SkillType.XiaoHao))
                ExhaustedCount++;
            else
                NonExhaustedCount++;

            if (entry.GetSkillTypeComposite().Contains(SkillType.Attack))
            {
                consecutiveAttackCount++;
                HighestConsecutiveAttackCount = Mathf.Max(HighestConsecutiveAttackCount, consecutiveAttackCount);

                AttackCount++;
            }
            else
            {
                NonAttackCount++;
                consecutiveAttackCount = 0;
            }

            TotalCostCount += entry.GetCostDescription(jingJie).ByType(CostDescription.CostType.Mana);
        }

        WuXingOrder = new List<WuXing>(WuXing.Traversal);
        WuXingOrder.Sort((lhs, rhs) => WuXingCounts[rhs] - WuXingCounts[lhs]);

        Proficiency = 0;
    }
}
