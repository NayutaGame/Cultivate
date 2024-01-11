
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class StateBehaviourPivot : StateBehaviour
{
    [NonSerialized] public RectTransform BaseTransform;

    [NonSerialized] public RectTransform PivotTransform;
    [NonSerialized] public RectTransform IdleTransform;
    [NonSerialized] public RectTransform HoverTransform;
    [NonSerialized] public RectTransform FollowTransform;

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
        BaseTransform.position = end.position;
        BaseTransform.localScale = end.localScale;
    }

    public void AnimateState(RectTransform start, RectTransform end)
    {
        SetState(start);
        AnimateState(end);
    }

    private void AnimateState(RectTransform end)
    {
        _handle?.Kill();
        FollowAnimation f = new FollowAnimation(BaseTransform, end);
        _handle = f.GetHandle();
        _handle.SetAutoKill().Restart();
    }

    public void RefreshPivots()
        => AnimateState(IdleTransform);
}
