
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HoverStrategyBlackFill : HoverStrategy
{
    private Image _target;

    private float IdleState = 0f;
    private float HoverState = 0.2f;

    public HoverStrategyBlackFill(Image target, float idleState, float hoverState)
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

    public void SetState(float end)
    {
        _handle?.Kill();
        _target.color = new Color(_target.color.r, _target.color.g, _target.color.b, end);
    }

    public void AnimateState(float start, float end)
    {
        SetState(start);
        AnimateState(end);
    }

    public void AnimateState(float end)
    {
        _handle?.Kill();
        _handle = _target.DOFade(end, 0.15f);
        _handle.SetAutoKill().Restart();
    }
}
