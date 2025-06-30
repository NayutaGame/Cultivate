
public class StartStepDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int P;

    public StartStepDetails(StageEnvironment env, StageEntity owner, int p) : base(env)
    {
        Owner = owner;
        P = p;
    }
}
