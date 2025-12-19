
using System;
using DG.Tweening;

public class CarrierSlotView : SlotView
{
    public Func<Tween> EnterIdleFunc;
    public Func<Tween> EnterHoverFunc;

    protected override Tween EnterIdle()
        => EnterIdleFunc();

    protected override Tween EnterHover()
        => EnterHoverFunc();

    protected override Tween EnterFollow()
        => DOTween.Sequence()
            .Append(FollowAnimation.FromDefault(GetContentView().GetRect(), GetRect()).GetHandle());

    protected override Tween EnterIdleUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(EnterIdleFunc());

    protected override Tween EnterHoverUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(EnterHoverFunc());

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
}