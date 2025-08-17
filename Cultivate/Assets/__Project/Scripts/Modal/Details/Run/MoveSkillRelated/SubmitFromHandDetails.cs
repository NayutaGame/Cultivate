
public class SubmitFromHandDetails : RunClosureDetails
{
    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;
    public RunSkill Skill;
    public RequirementSlot RequirementSlot;
    public bool IsReplace;
    
    public SubmitFromHandDetails(RunSkill skill, RequirementSlot requirementSlot)
    {
        Skill = skill;
        RequirementSlot = requirementSlot;

        FromDeckIndex = Skill.ToDeckIndex();
        ToDeckIndex = RequirementSlot.ToDeckIndex();
        
        IsReplace = RequirementSlot.Skill != null;
    }
}
