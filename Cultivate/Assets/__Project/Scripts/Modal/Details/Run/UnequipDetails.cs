
public class UnequipDetails : RunClosureDetails
{
    public DeckIndex DeckIndex;
    public SkillSlot SkillSlot;

    private UnequipDetails(SkillSlot skillSlot, DeckIndex deckIndex)
    {
        SkillSlot = skillSlot;
        DeckIndex = deckIndex;
    }

    public static UnequipDetails FromSlot(SkillSlot slot)
        => new(slot, slot.ToDeckIndex());

    public static UnequipDetails FromDeckIndex(DeckIndex deckIndex)
        => new(RunManager.Instance.Environment.SlotFromDeckIndex(deckIndex), deckIndex);
}
