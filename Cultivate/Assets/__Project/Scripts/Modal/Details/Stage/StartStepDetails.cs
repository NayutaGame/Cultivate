
public class StartStepDetails : ClosureDetails
{
    public StageEntity Owner;
    public int P;

    public StartStepDetails(StageEntity owner, int p)
    {
        Owner = owner;
        P = p;
    }
}
