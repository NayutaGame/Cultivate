
public class LoseHealthDetails : ClosureDetails
{
    public StageEntity Owner;
    public int Value;

    public LoseHealthDetails(StageEntity owner, int value, bool induced)
    {
        Owner = owner;
        Value = value;
        Induced = induced;
    }
}
