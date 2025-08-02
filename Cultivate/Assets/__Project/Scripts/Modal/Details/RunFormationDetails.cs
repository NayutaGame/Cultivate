
public class RunFormationDetails : RunClosureDetails
{
    public RunEntity Owner;

    public int[] WuXingCounts;
    public int[] TypeCounts;

    public int Proficiency;

    public RunFormationDetails(RunEntity entity)
    {
        Owner = entity;

        WuXingCounts = new int[WuXing.Length];
        TypeCounts = new int[TagCategory.Length];

        foreach (SkillSlot slot in entity.TraversalCurrentSlots())
        {
            SkillEntry entry = slot.PlacedSkill.Entry;
            JingJie jingJie = slot.PlacedSkill.JingJie;

            WuXing wuXing = entry.WuXing;
            if (wuXing.IsBasic())
                WuXingCounts[wuXing.GetIndex()]++;
            
            long tagComposite = entry.GetTagComposite().Value;
            for (int i = 0; i < TagCategory.Length; i++)
            {
                if ((tagComposite & 0b1) == 1)
                    TypeCounts[i]++;

                tagComposite = tagComposite >> 1;
            }
        }
        
        Proficiency = 0;
    }
}
