
public class LoseBuffDetails : StageClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry BuffEntry;
    public int Stack;
    public bool Recursive;

    public LoseBuffDetails(
        StageEnvironment env,
        StageEntity src,
        StageEntity tgt,
        BuffEntry buffEntry,
        int stack,
        bool recursive,
        bool induced) : base(env, induced)
    {
        Src = src;
        Tgt = tgt;
        BuffEntry = buffEntry;
        Stack = stack;
        Recursive = recursive;
    }
}
