
using DG.Tweening;
using UnityEngine;

public class ScaleSlotView : SlotView
{
    [SerializeField] public Configuration IdleConfiguration = new(localScale: Vector3.one);
    [SerializeField] public Configuration HoverConfiguration = new(localScale: 1.2f * Vector3.one);
    
    protected override Tween EnterIdle()
        => DOTween.Sequence()
            .Append(GoToConfiguration(IdleConfiguration));

    protected override Tween EnterHover()
        => DOTween.Sequence()
            .Append(GoToConfiguration(HoverConfiguration));

    protected override Tween EnterFollow()
        => DOTween.Sequence()
            .Append(RigidAnimation.FromFollow(GetRect(), GetContentView().GetRect()).GetHandle());

    protected override Tween EnterIdleUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(GoToConfiguration(IdleConfiguration));

    protected override Tween EnterHoverUseGrabber()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetHover)
            .Append(GoToConfiguration(HoverConfiguration));

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

    private Tween GoToConfiguration(Configuration configuration)
    {
        return new GoToConfigurationAnimation(GetRect(), GetContentView().GetRect(), configuration).GetHandle();
    }
}