
using System;
using CLLibrary;
using DG.Tweening;

public class TableSM : StateMachine<int>
{
    private Table<Func<Tween>> _table;
    public Func<Tween> this[int from, int to]
    {
        get => _table[from, to];
        set => _table[from, to] = value;
    }

    public Tween SetStateTween(int value)
    {
        Sequence seq = DOTween.Sequence();
        
        if (_table[State, -1] is { } first)
            seq.Append(first());

        if (_table[State, value] is { } second)
            seq.Append(second());

        seq.AppendCallback(() => base.SetState(value));

        if (_table[-1, value] is { } third)
            seq.Append(third());

        return seq;
    }

    public TableSM(int size)
    {
        _table = new(size);
    }
}
