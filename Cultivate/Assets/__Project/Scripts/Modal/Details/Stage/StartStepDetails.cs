
public class StartStepDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int P;

    public StartStepDetails(StageEntity owner, int p)
    {
        Owner = owner;
        P = p;
    }
}
