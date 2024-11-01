
public class PickDiscoveredSkillDetails : ClosureDetails
{
    public DeckIndex DeckIndex;
    public int PickedIndex;
    
    public PickDiscoveredSkillDetails(DeckIndex deckIndex, int pickedIndex)
    {
        DeckIndex = deckIndex;
        PickedIndex = pickedIndex;
    }
}
