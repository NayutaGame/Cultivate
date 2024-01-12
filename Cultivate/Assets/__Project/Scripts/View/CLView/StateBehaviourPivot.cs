
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StateBehaviourPivot : StateBehaviour
{
    public RectTransform GetDisplayTransform()
        => CLView.GetDisplayTransform();

    public void SetDisplayTransform(RectTransform pivot)
        => CLView.SetDisplayTransform(pivot);

    public RectTransform IdleTransform;
    public RectTransform HoverTransform;
    public RectTransform FollowTransform;

    public override void PointerEnter(CLView v, PointerEventData d)
    {
        AnimateState(HoverTransform);
    }

    public override void PointerExit(CLView v, PointerEventData d)
    {
        AnimateState(IdleTransform);
    }

    public override void PointerMove(CLView v, PointerEventData d)
    {
    }

    private void SetState(RectTransform end)
    {
        _handle?.Kill();
        SetDisplayTransform(end);
    }

    public void AnimateState(RectTransform start, RectTransform end)
    {
        SetState(start);
        AnimateState(end);
    }

    private void AnimateState(RectTransform end)
    {
        _handle?.Kill();
        FollowAnimation f = new FollowAnimation(GetDisplayTransform(), end);
        _handle = f.GetHandle();
        _handle.SetAutoKill().Restart();
    }

    public void RefreshPivots()
        => AnimateState(IdleTransform);
}
