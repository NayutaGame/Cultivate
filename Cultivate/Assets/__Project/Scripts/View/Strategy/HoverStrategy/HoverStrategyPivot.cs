
using DG.Tweening;
using UnityEngine;

public class HoverStrategyPivot : HoverStrategy
{
    private RectTransform _target;

    private RectTransform IdleState;
    private RectTransform HoverState;

    public HoverStrategyPivot(RectTransform target, RectTransform idleState, RectTransform hoverState)
    {
        _target = target;
        IdleState = idleState;
        HoverState = hoverState;
    }

    protected override void SetStateToIdle()
    {
        SetState(IdleState);
    }

    protected override void AnimateStateToIdle()
    {
        AnimateState(IdleState);
    }

    protected override void AnimateStateToHover()
    {
        AnimateState(HoverState);
    }

    public void SetState(RectTransform end)
    {
        _handle?.Kill();
        _target.position = end.position;
        _target.localScale = end.localScale;
    }

    public void AnimateState(RectTransform start, RectTransform end)
    {
        SetState(start);
        AnimateState(end);
    }

    public void AnimateState(RectTransform end)
    {
        _handle?.Kill();
        FollowAnimation f = new FollowAnimation(_target, end);
        _handle = f.GetHandle();
        _handle.SetAutoKill().Restart();
    }
}
