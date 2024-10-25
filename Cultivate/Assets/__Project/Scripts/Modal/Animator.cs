
using System;
using Cysharp.Threading.Tasks;
using CLLibrary;
using DG.Tweening;

public class Animator : StateMachine<int>
{
    private Table<Func<Tween>> _table;
    public Func<Tween> this[int from, int to]
    {
        get => _table[from, to];
        set => _table[from, to] = value;
    }
    
    private Tween _handle;

    public Animator(int size)
    {
        _table = new(size);
    }

    public Tween SetStateTween(int state)
    {
        Sequence seq = DOTween.Sequence();
        if (this[State, -1] is { } first)
            seq.Append(first());

        if (this[State, state] is { } second)
            seq.Append(second());
        
        seq.AppendCallback(() => base.SetState(state));

        if (this[-1, state] is { } third)
            seq.Append(third());

        return seq;
    }

    public override void SetState(int state)
    {
        _handle?.Kill();
        _handle = SetStateTween(state).SetAutoKill();
        _handle.Complete(true);
    }

    public async UniTask SetStateAsync(int state)
    {
        _handle?.Kill();
        _handle = SetStateTween(state).SetAutoKill();
        _handle.Restart();
        await _handle.AsyncWaitForCompletion();
    }
}
