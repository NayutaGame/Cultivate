using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MaskView : MonoBehaviour, IShowable
{
    [SerializeField] private Image _image;

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

    public Tween ShowAnimation()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(_image.DOFade(0.8f, 0.4f).SetEase(Ease.OutQuad));

    public Tween HideAnimation()
        => DOTween.Sequence()
            .Append(_image.DOFade(0f, 0.4f).SetEase(Ease.InQuad))
            .AppendCallback(() => gameObject.SetActive(false));

    #endregion
}
