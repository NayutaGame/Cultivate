
public class GainBuffDetails : ClosureDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry _buffEntry;
    public int _stack;
    public bool _recursive;

    public GainBuffDetails(
        StageEntity src,
        StageEntity tgt,
        BuffEntry buffEntry,
        int stack,
        bool recursive,
        bool induced)
    {
        Src = src;
        Tgt = tgt;
        _buffEntry = buffEntry;
        _stack = stack;
        _recursive = recursive;
        Induced = induced;
    }

    public GainBuffDetails Clone() => new(Src, Tgt, _buffEntry, _stack, _recursive, Induced);
}
