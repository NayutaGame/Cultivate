
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XBehaviourBlackFill : XBehaviour
{
    [SerializeField] private Image _target;

    [SerializeField] [Range(0, 1)] private float IdleState = 0f;
    [SerializeField] [Range(0, 1)] private float HoverState = 0.2f;

    private Tween _handle;

    public override void Init(XView view)
    {
        base.Init(view);

        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        AnimateState(HoverState);
    }

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        AnimateState(IdleState);
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
