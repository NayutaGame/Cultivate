
public class NestedStageClosureDetails : StageClosureDetails
{
    public bool HasRegistered = false;
    
    public StageClosureListener Listener;
    public StageClosure[] Closures;
    public ResultDict CastResult;

    public NestedStageClosureDetails(StageEnvironment env, bool induced = false) : base(env, induced)
    {
        
    }
}