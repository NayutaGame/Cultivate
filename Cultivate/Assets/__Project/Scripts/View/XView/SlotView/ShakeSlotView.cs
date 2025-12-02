
using DG.Tweening;
using UnityEngine;

public class ShakeSlotView : SlotView
{
    protected override Tween EnterIdle()
        => DOTween.Sequence().Append(GetRotateToIdentityTween());

    protected override Tween EnterHover()
        => DOTween.Sequence().Append(GetShakeTween());

    protected override Tween EnterFollow()
        => DOTween.Sequence()
            .Append(new FollowAnimation(GetContentView().GetRect(), GetRect()).GetHandle());

    protected override Tween EnterIdleUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(GetRotateToIdentityTween());

    protected override Tween EnterHoverUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetHover)
            .Append(GetShakeTween());

    protected override Tween EnterFollowUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetDrag)
            .Append(new FollowAnimation(GetContentView().GetRect(), CanvasManager.Instance.GetGrabber().GetRect()).GetHandle());

    protected override Tween EnterFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(false));

    protected override Tween ExitFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(true));

    private Tween GetRotateToIdentityTween()
        => GetContentView().GetRect().DORotateQuaternion(Quaternion.identity, 0.2f).SetEase(Ease.OutQuad);

    private Tween GetShakeTween()
        => new ShakeAnimation(GetRect(), GetContentView().GetRect()).GetHandle();
}