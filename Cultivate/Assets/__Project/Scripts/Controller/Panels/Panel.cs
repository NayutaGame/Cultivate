
using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public abstract class Panel : MonoBehaviour, IShowable
{
    [NonSerialized] public RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        Configure();
    }

    public virtual void Configure() { }
    public virtual void Refresh() { }

    #region IShowable

    private bool _showing;
    public virtual bool IsShowing() => _showing;
    public virtual async Task SetShowing(bool showing)
    {
        if (_showing == showing)
            return;

        _showing = showing;
        await PlayTween(false, _showing ? ShowAnimation() : HideAnimation());
    }

    private async Task PlayTween(bool isAwait, Tween tween)
    {
        _showHandle?.Kill();
        _showHandle = tween;
        // _showHandle.timeScale = _speed;
        _showHandle.SetAutoKill().Restart();
        if (isAwait)
            await _showHandle.AsyncWaitForCompletion();
    }

    private Tween _showHandle;

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

    #endregion
}
