
using System;
using Cysharp.Threading.Tasks;
using CLLibrary;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class Animator : StateMachine<int>
{
    private Table<Func<Tween>> _table;
    public Func<Tween> this[int from, int to]
    {
        get => _table[from, to];
        set => _table[from, to] = value;
    }

    private string _id;
    private Tween _handle;

    public Animator(int size, string id = null)
    {
        _table = new(size);
        _id = id ?? "Anonymous";
    }

    public Tween SetStateTween(int state)
    {
        Sequence seq = DOTween.Sequence();
        if (this[State, -1] is { } first)
            seq.Append(first());

        if (this[State, state] is { } second)
            seq.Append(second());
        
        // Debug.Log($"{_id}: {State} => {state}");
        seq.AppendCallback(() => base.SetState(state));

        if (this[-1, state] is { } third)
            seq.Append(third());

        return seq;
    }

    public override void SetState(int state)
        => SetTween(SetStateTween(state));

    public async UniTask SetStateAsync(int state)
        => await SetTweenAsync(SetStateTween(state));

    public async UniTask ToBeFinished()
    {
        if (_handle == null)
            return;
        await _handle.AsyncWaitForCompletion();
    }

    public void SetTween(Tween tween)
    {
        _handle?.Kill();
        _handle = tween.SetAutoKill();
        _handle.Complete(true);
    }

    public async UniTask SetTweenAsync(Tween tween)
    {
        _handle?.Kill();
        _handle = tween.SetAutoKill();
        _handle.Restart();
        await _handle.AsyncWaitForCompletion();
    }
    
    public bool IsAnimating => _handle != null && _handle.active;
}
