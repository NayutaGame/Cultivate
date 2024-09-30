
public class ActionDetails : ClosureDetails
{
    public StageEntity Owner;
    public int CurrActionPoint;

    public ActionDetails(StageEntity owner, int currActionPoint)
    {
        Owner = owner;
        CurrActionPoint = currActionPoint;
    }
    
    public bool IsSwift => CurrActionPoint > 1;
}
