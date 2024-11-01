
public class BuySkillDetails : ClosureDetails
{
    public DeckIndex DeckIndex;
    public int CommodityIndex;
    
    public BuySkillDetails(DeckIndex deckIndex, int commodityIndex)
    {
        DeckIndex = deckIndex;
        CommodityIndex = commodityIndex;
    }
}
