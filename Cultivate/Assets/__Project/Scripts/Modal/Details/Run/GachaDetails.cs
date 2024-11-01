
public class GachaDetails : ClosureDetails
{
    public DeckIndex DeckIndex;
    public int GachaIndex;
    
    public GachaDetails(DeckIndex deckIndex, int gachaIndex)
    {
        DeckIndex = deckIndex;
        GachaIndex = gachaIndex;
    }
}
