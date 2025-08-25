
using System.Collections.Generic;

public class DepleteDetails : RunClosureDetails
{
    public RunEntity Owner;
    public bool PreserveFirstDeplete;
    public List<SkillSlot> DepletingSlots;
    public List<RunSkill> DepletedSkills;

    public DepleteDetails(RunEntity owner)
    {
        Owner = owner;
        PreserveFirstDeplete = false;
        CalcDepletingSlots();
        DepletedSkills = new();
    }

    private void CalcDepletingSlots()
    {
        DepletingSlots = new();        

        int count = Owner.GetSlotCount();
        for (int i = 0; i < count; i++)
        {
            SkillSlot slot = Owner.GetSlot(i);
            RunSkill skill = slot.Skill;
            
            if (skill == null)
                continue;
            
            bool depleted = skill.GetEntry().GetTagComposite().Contains(TagCategory.Deplete);
            if (!depleted)
                continue;
            
            DepletingSlots.Add(slot);
        }
    }
}
