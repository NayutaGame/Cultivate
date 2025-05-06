
public class GainBuffDetails : NestedStageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;

    public GainBuffDetails(
        StageEntity src,
        StageEntity tgt,
        BuffEntry buffEntry,
        int stack,
        bool recursive,
        StageClosureListener listener,
        ResultDict castResult,
        StageClosure[] closures,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
        Listener = listener;
        CastResult = castResult;
        Closures = closures;
        Induced = induced;
    }

    public GainBuffDetails ShallowClone() => new(Src, Tgt, BuffEntry, Stack, Recursive, Listener, CastResult, Closures, Induced);
}
