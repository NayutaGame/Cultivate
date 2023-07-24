using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry _buffEntry;
    public int _stack;
    public bool _friendly;
    public bool _recursive;
    public bool Cancel;

    public ConsumeDetails(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool friendly = true, bool recursive = true, bool cancel = false)
    {
        Src = src;
        Tgt = tgt;
        _buffEntry = buffEntry;
        _stack = stack;
        _friendly = friendly;
        _recursive = recursive;
        Cancel = cancel;
    }
}
