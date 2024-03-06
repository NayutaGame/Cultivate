
public class TurnDetails : EventDetails
{
    public StageEntity Owner;
    public int SlotIndex;

    public TurnDetails(StageEntity owner, int slotIndex)
    {
        Owner = owner;
        SlotIndex = slotIndex;
    }
}
