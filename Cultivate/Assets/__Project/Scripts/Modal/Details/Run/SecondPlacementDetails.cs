
public class SecondPlacementDetails : ClosureDetails
{
    public RunEntity Owner;
    public SlotListModel Slots;

    public SecondPlacementDetails(RunEntity owner, SlotListModel slots)
    {
        Owner = owner;
        Slots = slots;
    }
}
