
using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class Panel : MonoBehaviour, IShowable
{
    [NonSerialized] public RectTransform _rectTransform;

    public virtual void Configure()
    {
        _rectTransform ??= GetComponent<RectTransform>();
    }

    public virtual void Refresh() { }

    #region IShowable

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

    private async Task PlayTween(bool isAwait, Tween tween)
    {
        _handle?.Kill();
        _handle = tween;
        // _showHandle.timeScale = _speed;
        _handle.SetAutoKill().Restart();
        if (isAwait)
            await _handle.AsyncWaitForCompletion();
    }

    private Tween _handle;

    public virtual Tween ShowAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true));
    }

    public virtual Tween HideAnimation()
    {
        return DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(false));
    }

    public void SetHideState()
    {
        Tween t = HideAnimation().SetAutoKill();
        t.Restart();
        t.Complete();
    }

    #endregion
}
