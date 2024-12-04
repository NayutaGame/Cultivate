
public class EquipDetails : ClosureDetails
{
    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;
    public RunSkill Skill;
    public SkillSlot SkillSlot;
    public bool IsReplace;
    
    public EquipDetails(RunSkill skill, SkillSlot skillSlot)
    {
        Skill = skill;
        SkillSlot = skillSlot;

        FromDeckIndex = Skill.ToDeckIndex();
        ToDeckIndex = SkillSlot.ToDeckIndex();
        
        IsReplace = false;
    }
}
