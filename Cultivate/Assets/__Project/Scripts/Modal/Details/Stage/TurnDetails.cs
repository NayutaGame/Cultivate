
public class TurnDetails : EventDetails
{
    public StageEntity Owner;
    public int TurnCount;

    public TurnDetails(StageEntity owner, int turnCount)
    {
        Owner = owner;
        TurnCount = turnCount;
    }
}
