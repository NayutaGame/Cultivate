
public class NestedStageClosureDetails : StageClosureDetails
{
    public bool HasRegistered = false;
    
    public StageClosureListener Listener;
    public StageClosure[] Closures;
    public ResultDict CastResult;
}