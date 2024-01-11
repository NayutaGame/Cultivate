using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateBehaviourBlackFill : StateBehaviour
{
    [SerializeField] private Image _target;

    [SerializeField] [Range(0, 1)] private float IdleState = 0f;
    [SerializeField] [Range(0, 1)] private float HoverState = 0.2f;

    public override void PointerEnter(CLView v, PointerEventData d)
    {
        AnimateState(HoverState);
    }

    public override void PointerExit(CLView v, PointerEventData d)
    {
        AnimateState(IdleState);
    }

    public override void PointerMove(CLView v, PointerEventData d)
    {
    }

    private void SetState(float end)
    {
        _handle?.Kill();
        _target.color = new Color(_target.color.r, _target.color.g, _target.color.b, end);
    }

    private void AnimateState(float start, float end)
    {
        SetState(start);
        AnimateState(end);
    }

    private void AnimateState(float end)
    {
        _handle?.Kill();
        _handle = _target.DOFade(end, 0.15f);
        _handle.SetAutoKill().Restart();
    }
}
