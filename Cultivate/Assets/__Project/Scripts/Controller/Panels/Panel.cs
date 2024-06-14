
using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class Panel : MonoBehaviour
{
    [NonSerialized] public RectTransform RectTransform;

    public virtual void Configure()
    {
        RectTransform ??= GetComponent<RectTransform>();
        InitStateMachine();
    }

    protected virtual void InitStateMachine()
    {
        SM = new(2);
        // 0 for hide, 1 for show
        SM[0, 1] = ShowTween;
        SM[-1, 0] = HideTween;
    }

    public virtual void Refresh() { }

    public TableSM SM;
    private Tween _handle;

    public Tween SetStateTween(int state)
        => SM.SetStateTween(state);

    public void SetState(int state)
    {
        SetStateTween(state).SetAutoKill().Complete(true);
    }

    public async Task SetStateAsync(int state)
    {
        _handle?.Kill();
        _handle = SetStateTween(state);
        _handle.SetAutoKill().Restart();
        await _handle.AsyncWaitForCompletion();
    }

    public async Task ToggleShowing()
        => await SetStateAsync(SM.State != 0 ? 0 : 1);

    public virtual Tween ShowTween()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(true));

    public virtual Tween HideTween()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));
}
