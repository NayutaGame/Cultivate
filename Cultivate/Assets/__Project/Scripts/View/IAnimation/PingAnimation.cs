
using System;
using DG.Tweening;
using UnityEngine;

public class PingAnimation : FromCurrentAnimation
{
    private Vector3 TargetScale;
    private Vector3 EndScale;
    private float Duration;

    public PingAnimation(RectTransform content, Vector3 targetScale, float duration) : base(content)
    {
        TargetScale = targetScale;
        EndScale = Vector3.one;
        Duration = duration;
    }

    public override Tween GetHandle()
    {
        throw new NotImplementedException();
        // return DOTween.Sequence()
        //     .Append(Content.DOScale(TargetScale, Duration / 2).SetEase(Ease.OutQuad))
        //     .Append(Content.DOScale(EndScale, Duration / 2).SetEase(Ease.InQuad));
    }

    protected override void SetProgress(float t)
    {
        throw new NotImplementedException();
    }
}