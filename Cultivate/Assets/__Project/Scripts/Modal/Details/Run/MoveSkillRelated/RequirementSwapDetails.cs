
public class RequirementSwapDetails : RunClosureDetails
{
    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;
    public RequirementSlot FromSlot;
    public RequirementSlot ToSlot;
    public bool IsReplace;
    
    public RequirementSwapDetails(RequirementSlot fromSlot, RequirementSlot toSlot)
    {
        FromSlot = fromSlot;
        ToSlot = toSlot;

        FromDeckIndex = FromSlot.ToDeckIndex();
        ToDeckIndex = ToSlot.ToDeckIndex();
        
        IsReplace = ToSlot.Skill != null;
    }
}
