
public class StageDetails : StageClosureDetails
{
    public StageEntity Owner;

    public StageDetails(StageEnvironment env, StageEntity owner) : base(env)
    {
        Owner = owner;
    }
}
