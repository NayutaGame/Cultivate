
public class ActionDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int CurrActionPoint;

    public ActionDetails(StageEnvironment env, StageEntity owner, int currActionPoint) : base(env)
    {
        Owner = owner;
        CurrActionPoint = currActionPoint;
    }
    
    public bool IsSwift => CurrActionPoint > 0;
}
