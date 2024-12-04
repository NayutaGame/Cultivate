
public class SwapDetails
{
    public DeckIndex FromDeckIndex;
    public DeckIndex ToDeckIndex;
    public SkillSlot FromSlot;
    public SkillSlot ToSlot;
    public bool IsReplace;
    
    public SwapDetails(SkillSlot fromSlot, SkillSlot toSlot)
    {
        FromSlot = fromSlot;
        ToSlot = toSlot;

        FromDeckIndex = FromSlot.ToDeckIndex();
        ToDeckIndex = ToSlot.ToDeckIndex();
        
        IsReplace = false;
    }
}
