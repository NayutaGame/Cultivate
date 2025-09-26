
using System;

public class NestedStageClosureDetails : StageClosureDetails
{
    public StageClosureListener Listener;
    public StageClosure[] Closures;
    public ResultDict CastResult;
    public bool ClosureHasRegistered;

    public NestedStageClosureDetails(
        StageEnvironment env,
        StageClosureListener listener,
        StageClosure[] closures,
        ResultDict castResult,
        bool closureHasRegistered,
        bool induced = false) : base(env, induced)
    {
        Listener = listener;
        Closures = closures ?? Array.Empty<StageClosure>();
        CastResult = castResult;
        ClosureHasRegistered = closureHasRegistered;
    }
}