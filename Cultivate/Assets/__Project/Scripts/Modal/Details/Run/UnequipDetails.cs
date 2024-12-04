
public class UnequipDetails : ClosureDetails
{
    public DeckIndex FromDeckIndex;
    public SkillSlot SkillSlot;
    
    public UnequipDetails(SkillSlot skillSlot)
    {
        SkillSlot = skillSlot;

        FromDeckIndex = SkillSlot.ToDeckIndex();
    }
}
