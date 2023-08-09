using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationArguments
{
    public int[] WuXingCounts;
    public int SwiftCount;
    public int ExhaustedCount;
    public int NonAttackCount;
    public int TotalCostCount;
    public int HighestConsecutiveAttackCount;

    public FormationArguments(RunEntity entity)
    {
        GenerateArguments(entity);
    }

    private void GenerateArguments(RunEntity entity)
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
            if (slot.Skill == null)
            {
                consecutiveAttackCount = 0;
                continue;
            }

            if (slot.Skill.Entry.WuXing != null)
                WuXingCounts[slot.Skill.Entry.WuXing.Value]++;

            if (slot.Skill.GetSkillTypeComposite().Contains(SkillType.ErDong))
                SwiftCount++;

            if (slot.Skill.GetSkillTypeComposite().Contains(SkillType.XiaoHao))
                ExhaustedCount++;

            if (slot.Skill.GetSkillTypeComposite().Contains(SkillType.Attack))
            {
                consecutiveAttackCount++;
                HighestConsecutiveAttackCount = Mathf.Max(HighestConsecutiveAttackCount, consecutiveAttackCount);
            }
            else
            {
                consecutiveAttackCount = 0;
                NonAttackCount++;
            }

            TotalCostCount += slot.Skill.GetManaCost();
        }
    }
}
