
public class SlotListModel : ListModel<SkillSlot>
{
    public static SlotListModel Default()
    {
        SlotListModel ret = new();
        
        for (int i = 0; i < RunManager.MaxSlotCount; i++)
        {
            SkillSlot slot = new SkillSlot(i);
            ret.Add(slot);
        }

        return ret;
    }

    public static SlotListModel FromSkills(RunSkill[] skills)
    {
        SlotListModel ret = new();

        for (int i = 0; i < skills.Length; i++)
        {
            SkillSlot slot = new SkillSlot(i);
            slot.Skill = skills[i];
            ret.Add(slot);
        }

        return ret;
    }

    public SlotListModel Clone()
    {
        SlotListModel ret = new();
        
        for (int i = 0; i < RunManager.MaxSlotCount; i++)
        {
            SkillSlot slot = new SkillSlot(i);
            slot.Skill = this[i].Skill;
            ret.Add(slot);
        }
        
        return ret;
    }
}
