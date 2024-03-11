
public class StartStepDetails : EventDetails
{
    public StageEntity Owner;
    public int P;

    public StartStepDetails(StageEntity owner, int p)
    {
        Owner = owner;
        P = p;
    }
}
