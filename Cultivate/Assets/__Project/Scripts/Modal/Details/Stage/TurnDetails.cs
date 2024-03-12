
public class TurnDetails : EventDetails
{
    public StageEntity Owner;

    public TurnDetails(StageEntity owner)
    {
        Owner = owner;
    }
}
