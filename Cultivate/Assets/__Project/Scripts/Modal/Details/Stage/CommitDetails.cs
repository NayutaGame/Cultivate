
public class CommitDetails : EventDetails
{
    public StageEntity Owner;
    public int Flag;

    public CommitDetails(StageEntity owner)
    {
        Owner = owner;
        Flag = 0;
    }
}
