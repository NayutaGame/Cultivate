
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

    public SlotListModel Clone()
    {
        SlotListModel ret = new();
        
        for (int i = 0; i < RunManager.MaxSlotCount; i++)
        {
            SkillSlot slot = new SkillSlot(i);
            // slot.Skill = this[i].Skill.Clone();
            slot.Skill = this[i].Skill;
            ret.Add(slot);
        }
        
        return ret;
    }
}
