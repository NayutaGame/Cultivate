
public class GainBuffDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;

    public GainBuffDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        BuffEntry buffEntry,
        int stack,
        bool recursive,
        StageClosureListener listener,
        ResultDict castResult,
        StageClosure[] closures,
        bool induced) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
    }

    public GainBuffDetails ShallowClone() => new(Env, Src, Tgt, BuffEntry, Stack, Recursive, Listener, CastResult, Closures, Induced);
}
