
using System;
using DG.Tweening;
using UnityEngine;

public class ShakeSlotView : SlotView
{
    [NonSerialized] public SlotOffset SlotOffset = SlotOffset.Default();
    
    public void SetFlipped(bool flipped)
        => SlotOffset.Rotation = !flipped ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

    protected override Tween EnterIdle()
        => GoToIdleTween();

    protected override Tween EnterHover()
        => GetShakeTween();

    protected override Tween EnterFollow()
        => RigidAnimation.FromFollow(GetRect(), GetContentView().GetRect()).GetHandle();

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
            .Append(RigidAnimation.FromFollow(CanvasManager.Instance.GetGrabber().GetRect(), GetContentView().GetRect()).GetHandle());

    protected override Tween EnterFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(false));

    protected override Tween ExitFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(true));

    private Tween GoToIdleTween()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(RigidAnimation.FromSlotOffset(GetRect(), GetContentView().GetRect(), SlotOffset).GetHandle());
        // blend
        // randomize phase
        int yFrequency = UnityEngine.Random.Range(3, 7);
        int duration = UnityEngine.Random.Range(30, 45);
        seq.Append(new LissajousAnimation(GetRect(), GetContentView().GetRect(), 0.04f, 0.1f, 2, yFrequency, duration).GetHandle()
            .SetLoops(99999));
        return seq;
    }

    private Tween GetShakeTween()
    {
        return new ShakeAnimation(GetRect(), GetContentView().GetRect()).GetHandle();
    }
}