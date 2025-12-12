
using System;
using DG.Tweening;
using UnityEngine;

public class PingAnimation : IAnimation
{
    private RectTransform Content;
    private Vector3 TargetScale;
    private Vector3 EndScale;
    private float Duration;

    public PingAnimation(RectTransform content, Vector3 targetScale, float duration)
    {
        Content = content;
        TargetScale = targetScale;
        EndScale = Vector3.one;
        Duration = duration;
    }

    public Tween GetHandle()
    {
        return DOTween.Sequence()
            .Append(Content.DOScale(TargetScale, Duration / 2).SetEase(Ease.OutQuad))
            .Append(Content.DOScale(EndScale, Duration / 2).SetEase(Ease.InQuad));
    }

    public void AppendHandle(Sequence seq)
    {
        seq.Append(GetHandle());
    }

    public void SetProgress(float t)
    {
        throw new NotImplementedException();
    }
}