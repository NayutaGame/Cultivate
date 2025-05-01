
public class GainBuffDetails : StageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;
    public StageClosureListener Initiator;
    public CastResult CastResult;
    public StageClosure[] Closures;

    public GainBuffDetails(
        StageEntity src,
        StageEntity tgt,
        BuffEntry buffEntry,
        int stack,
        bool recursive,
        StageClosureListener initiator,
        CastResult castResult,
        StageClosure[] closures,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
        Initiator = initiator;
        CastResult = castResult;
        Closures = closures;
        Induced = induced;
    }

    public GainBuffDetails ShallowClone() => new(Src, Tgt, BuffEntry, Stack, Recursive, Initiator, CastResult, Closures, Induced);
}
