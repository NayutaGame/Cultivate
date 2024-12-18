
public class SlotListModel : ListModel<SkillSlot>
{
    public static SlotListModel Default()
        => DefaultWithSize(RunEntity.MaxSlotCount);

    public static SlotListModel DefaultWithSize(int size)
    {
        SlotListModel ret = new();
        
        for (int i = 0; i < size; i++)
        {
            SkillSlot slot = new SkillSlot(i);
            ret.Add(slot);
        }

        return ret;
    }

    public static SlotListModel FromSkills(RunSkill[] skills)
    {
        SlotListModel ret = Default();

        for (int i = 0; i < skills.Length; i++)
        {
            SkillSlot slot = ret[i];
            slot.Skill = skills[i];
        }

        return ret;
    }

    public SlotListModel Clone()
    {
        SlotListModel ret = Default();

        for (int i = 0; i < RunEntity.MaxSlotCount; i++)
        {
            SkillSlot slot = ret[i];
            slot.Skill = this[i].Skill;
        }
        
        return ret;
    }
}
