
using System;
using CLLibrary;
using Cysharp.Threading.Tasks;

public class AppStateMachine
{
    private Table<Func<object, UniTask>> _table;
    public Func<object, UniTask> this[int from, int to]
    {
        get => _table[from, to];
        set => _table[from, to] = value;
    }
    
    private int _state;

    public AppStateMachine(int size)
    {
        _table = new(size);
    }

    public async UniTask SetStateAsync(int state, object args)
    {
        if (this[_state, -1] is { } first)
            await first(args);

        if (this[_state, state] is { } second)
            await second(args);
        
        _state = state;

        if (this[-1, state] is { } third)
            await third(args);
    }
}
