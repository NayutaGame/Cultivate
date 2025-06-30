
public class RoundDetails : StageClosureDetails
{
    public StageEntity Owner;

    public RoundDetails(StageEnvironment env, StageEntity owner) : base(env)
    {
        Owner = owner;
    }
}
