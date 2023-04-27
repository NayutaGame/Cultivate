
public class BuffDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry _buffEntry;
    public int _stack;
    public bool _recursive;
    public bool Cancel;

    public BuffDetails(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true, bool cancel = false)
    {
        Src = src;
        Tgt = tgt;
        _buffEntry = buffEntry;
        _stack = stack;
        _recursive = recursive;
        Cancel = cancel;
    }
}
