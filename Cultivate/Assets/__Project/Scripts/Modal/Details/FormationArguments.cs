using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationArguments
{
    public int[] WuXingCounts;
    public int SwiftCount;
    public int ConsumeCount;
    public int NonAttackCount;
    public int TotalCostCount;
    public int ConsecutiveAttackCount;

    public FormationArguments(RunEntity entity)
    {
        GenerateArguments(entity);
    }

    private void GenerateArguments(RunEntity entity)
    {
        WuXingCounts = new int[WuXing.Length];

        foreach (SkillSlot slot in entity.TraversalCurrentSlots())
        {
            if (slot.Skill == null)
                continue;
            if (slot.Skill.Entry.WuXing == null)
                continue;

            WuXingCounts[slot.Skill.Entry.WuXing.Value]++;
        }

        SwiftCount = 0;
        ConsumeCount = 0;
        NonAttackCount = 0;
        TotalCostCount = 0;
        ConsecutiveAttackCount = 0;
    }
}
