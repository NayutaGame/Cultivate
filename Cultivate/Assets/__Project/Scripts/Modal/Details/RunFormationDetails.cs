
public class RunFormationDetails : EventDetails
{
    public RunEntity Owner;

    public int[] WuXingCounts;
    public int[] TypeCounts;

    public int Proficiency;

    public RunFormationDetails(RunEntity entity)
    {
        Owner = entity;

        WuXingCounts = new int[WuXing.Length];
        TypeCounts = new int[SkillType.Length];

        foreach (SkillSlot slot in entity.TraversalCurrentSlots())
        {
            SkillEntry entry = slot.PlacedSkill.Entry;
            JingJie jingJie = slot.PlacedSkill.JingJie;

            WuXing? wuXing = entry.WuXing;
            if (wuXing != null)
                WuXingCounts[wuXing.Value]++;

            SkillTypeComposite skillTypeComposite = entry.GetSkillTypeComposite();

            for (int i = 0; i < SkillType.Length; i++)
            {
                if (skillTypeComposite.Contains(i))
                    TypeCounts[i]++;
            }
        }

        Proficiency = 0;
    }
}
