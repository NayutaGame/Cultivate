using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MaskView : MonoBehaviour, IShowable
{
    [SerializeField] private Image _image;

    private bool _showing;
    public bool IsShowing() => _showing;
    public void SetShowing(bool showing) => _showing = showing;

    public Tween GetShowTween()
        => DOTween.Sequence()
            .AppendCallback(() => gameObject.SetActive(true))
            .Append(_image.DOFade(0.8f, 0.4f).SetEase(Ease.OutQuad));

    public Tween GetHideTween()
        => DOTween.Sequence()
            .Append(_image.DOFade(0f, 0.4f).SetEase(Ease.InQuad))
            .AppendCallback(() => gameObject.SetActive(false));
}
