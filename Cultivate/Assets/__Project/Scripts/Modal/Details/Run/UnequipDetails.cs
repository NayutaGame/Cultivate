
public class UnequipDetails : RunClosureDetails
{
    public DeckIndex FromDeckIndex;
    public SkillSlot SkillSlot;
    
    public UnequipDetails(SkillSlot skillSlot)
    {
        SkillSlot = skillSlot;

        FromDeckIndex = SkillSlot.ToDeckIndex();
    }
}
