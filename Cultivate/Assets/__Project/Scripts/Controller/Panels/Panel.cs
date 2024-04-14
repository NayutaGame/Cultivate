
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
    }

    public virtual void Refresh() { }

    private bool _showing;
    public virtual bool IsShowing() => _showing;
    public virtual async Task SetShowing(bool showing)
    {
        if (_showing == showing)
            return;

        _showing = showing;
        await PlayTween(true, _showing ? ShowAnimation() : HideAnimation());
    }

    public async Task ToggleShowing()
        => await SetShowing(!IsShowing());

    public void SetShowingNoTween(bool showing)
    {
        _showing = showing;
        Tween t = _showing ? ShowAnimation() : HideAnimation();
        t.SetAutoKill();
        // t.Restart();
        t.Complete();
    }

    private async Task PlayTween(bool isAwait, Tween tween)
    {
        _handle?.Kill();
        _handle = tween;
        // _handle.timeScale = _speed;
        _handle.SetAutoKill().Restart();
        if (isAwait)
            await _handle.AsyncWaitForCompletion();
    }

    private Tween _handle;

    public virtual Tween ShowAnimation()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(true));

    public virtual Tween HideAnimation()
        => DOTween.Sequence().AppendCallback(() => gameObject.SetActive(false));
}
