using System;
using System.Collections;
using System.Collections.Generic;
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

    private bool _showing;
    public virtual bool IsShowing() => _showing;
    public virtual void SetShowing(bool showing) => _showing = showing;
    public void Toggle()
    {
        _showing = !_showing;
        (_showing ? GetShowTween() : GetHideTween()).Restart();
    }

    public virtual Tween GetShowTween()
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => gameObject.SetActive(true));
    }

    public virtual Tween GetHideTween()
    {
        return DOTween.Sequence().SetAutoKill()
            .AppendCallback(() => gameObject.SetActive(false));
    }
}
