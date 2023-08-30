using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispelDetails : EventDetails
{
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry _buffEntry;
    public int _stack;
    public bool _friendly;
    public bool _recursive;

    public DispelDetails(StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool friendly = true, bool recursive = true)
    {
        Src = src;
        Tgt = tgt;
        _buffEntry = buffEntry;
        _stack = stack;
        _friendly = friendly;
        _recursive = recursive;
    }
}
