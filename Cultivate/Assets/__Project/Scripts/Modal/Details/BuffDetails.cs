using System;
using System.Text;
using DG.Tweening;
using UnityEngine;

public class BuffDetails
{
    public StringBuilder Seq;
    public StageEntity Src;
    public StageEntity Tgt;
    public BuffEntry _buffEntry;
    public int _stack;
    public bool _recursive;
    public bool Cancel;

    public Action<int> Clean;

    public BuffDetails(StringBuilder seq, StageEntity src, StageEntity tgt, BuffEntry buffEntry, int stack = 1, bool recursive = true, bool cancel = false)
    {
        Seq = seq;
        Src = src;
        Tgt = tgt;
        _buffEntry = buffEntry;
        _stack = stack;
        _recursive = recursive;
        Cancel = cancel;
    }

    public void Core()
    {
        Buff same = Tgt.FindBuff(_buffEntry);

        int oldStack = same?.Stack ?? 0;

        if (same != null)
        {
            switch (_buffEntry.BuffStackRule)
            {
                case BuffStackRule.Wasted:
                    break;
                case BuffStackRule.Add:
                    Tgt.StackBuff(same, _stack);
                    break;
            }

            Seq.Append($"    {_buffEntry.Name}: {oldStack} -> {same.Stack}");
        }
        else
        {
            Buff buff = new Buff(Tgt, _buffEntry, _stack);
            Tgt.AddBuff(buff);

            Seq.Append($"    {_buffEntry.Name}: 0 -> {buff.Stack}");
        }
    }
}
