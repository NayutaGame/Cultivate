
using System;
using Cysharp.Threading.Tasks;
using CLLibrary;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class Animator
{
    private int _state;
    public int State => _state;
    
    private Table<Func<Tween>> _table;
    public Func<Tween> this[int from, int to]
    {
        get => _table[from, to];
        set => _table[from, to] = value;
    }

    private string _id;
    private Tween _handle;
    private bool _printStateChange;

    public Animator(int size, string id = null, bool printStateChange = false)
    {
        _table = new(size);
        _id = id ?? "Anonymous";
        _printStateChange = printStateChange;
    }

    public Tween TweenFromSetState(int state)
    {
        Sequence seq = DOTween.Sequence();
        if (this[_state, -1] is { } first)
            seq.Append(first());

        if (this[_state, state] is { } second)
            seq.Append(second());
        
        if (_printStateChange)
            Debug.Log($"{_state} -> {state}");
        seq.AppendCallback(() => _state = state);

        if (this[-1, state] is { } third)
            seq.Append(third());

        return seq;
    }

    public void SetState(int state)
        => SetTween(TweenFromSetState(state));

    public async UniTask SetStateAsync(int state)
        => await SetTweenAsync(TweenFromSetState(state));

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
