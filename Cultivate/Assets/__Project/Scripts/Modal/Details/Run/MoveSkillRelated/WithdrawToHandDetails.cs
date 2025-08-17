
public class WithdrawToHandDetails : RunClosureDetails
{
    public DeckIndex DeckIndex;
    public RequirementSlot RequirementSlot;

    private WithdrawToHandDetails(RequirementSlot requirementSlot, DeckIndex deckIndex)
    {
        RequirementSlot = requirementSlot;
        DeckIndex = deckIndex;
    }

    public static WithdrawToHandDetails FromSlot(RequirementSlot slot)
        => new(slot, slot.ToDeckIndex());
}
