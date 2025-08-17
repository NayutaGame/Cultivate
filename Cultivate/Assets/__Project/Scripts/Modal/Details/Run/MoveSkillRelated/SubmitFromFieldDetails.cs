
public class SubmitFromFieldDetails : RunClosureDetails
{
    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;
    public SkillSlot FromSlot;
    public RequirementSlot ToSlot;
    public bool IsReplace;
    
    public SubmitFromFieldDetails(SkillSlot fromSlot, RequirementSlot toSlot)
    {
        FromSlot = fromSlot;
        ToSlot = toSlot;

        FromDeckIndex = FromSlot.ToDeckIndex();
        ToDeckIndex = ToSlot.ToDeckIndex();
        
        IsReplace = ToSlot.Skill != null;
    }
}
