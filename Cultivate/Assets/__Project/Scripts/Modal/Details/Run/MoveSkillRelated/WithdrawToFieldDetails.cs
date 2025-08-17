
public class WithdrawToFieldDetails : RunClosureDetails
{
    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;
    public RequirementSlot FromSlot;
    public SkillSlot ToSlot;
    public bool IsReplace;

    public WithdrawToFieldDetails(RequirementSlot fromSlot, SkillSlot toSlot)
    {
        FromSlot = fromSlot;
        ToSlot = toSlot;

        FromDeckIndex = FromSlot.ToDeckIndex();
        ToDeckIndex = ToSlot.ToDeckIndex();
    
        IsReplace = ToSlot.Skill != null;
    }
}
