
using System;
using DG.Tweening;
using UnityEngine;

public class ShakeSlotView : SlotView
{
    [NonSerialized] public Configuration Configuration = Configuration.Default();
    
    public void SetFlipped(bool flipped)
        => Configuration.Rotation = !flipped ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

    protected override Tween EnterIdle()
        => GoToIdleTween();

    protected override Tween EnterHover()
        => GetShakeTween();

    protected override Tween EnterFollow()
        => FollowAnimation.FromDefault(GetContentView().GetRect(), GetRect()).GetHandle();

    protected override Tween EnterIdleUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(GoToIdleTween());

    protected override Tween EnterHoverUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetHover)
            .Append(GetShakeTween());

    protected override Tween EnterFollowUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetDrag)
            .Append(FollowAnimation.FromDefault(GetContentView().GetRect(), CanvasManager.Instance.GetGrabber().GetRect()).GetHandle());

    protected override Tween EnterFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(false));

    protected override Tween ExitFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(true));

    private Tween GoToIdleTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(FollowAnimation.FromConfiguration(GetContentView().GetRect(), GetRect(), Configuration).GetHandle());
        seq.Append(LissajousAnimation.FromBreathPattern(GetContentView().GetRect(), GetRect()).GetHandle().SetLoops(99999));
        return seq;
    }

    private Tween GetShakeTween()
    {
        return new ShakeAnimation(GetContentView().GetRect(), GetRect()).GetHandle();
    }
}