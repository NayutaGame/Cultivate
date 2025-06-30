
public class TurnDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int TurnCount;

    public TurnDetails(StageEnvironment env, StageEntity owner, int turnCount) : base(env)
    {
        Owner = owner;
        TurnCount = turnCount;
    }
}
