
using System;
using CLLibrary;

public class TableSM : StateMachine<int>
{
    private Table<Action> _table;
    public Action this[int from, int to]
    {
        get => _table[from, to];
        set => _table[from, to] = value;
    }

    public TableSM(int size)
    {
        _table = new(size);
    }

    public override void SetState(int state)
    {
        if (this[State, -1] is { } first)
            first.Invoke();

        if (this[State, state] is { } second)
            second.Invoke();
        
        base.SetState(state);

        if (this[-1, state] is { } third)
            third.Invoke();
    }
}
