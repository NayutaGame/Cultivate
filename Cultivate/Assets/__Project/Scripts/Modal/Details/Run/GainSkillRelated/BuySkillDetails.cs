
public class BuySkillDetails : RunClosureDetails
{
    public Commodity Commodity;
    public int CommodityIndex;

    public DeckIndex DeckIndex;
    
    public BuySkillDetails(Commodity commodity, int commodityIndex)
    {
        Commodity = commodity;
        CommodityIndex = commodityIndex;
    }
}
