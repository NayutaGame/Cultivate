
public class CommitDetails : ClosureDetails
{
    public StageEntity Owner;
    public int Flag;

    public CommitDetails(StageEntity owner)
    {
        Owner = owner;
        Flag = 0;
    }
}
