
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostView : MonoBehaviour
{
    public SimpleView SimpleView;

    public void Awake()
    {
        SimpleView ??= GetComponent<SimpleView>();
        SimpleView.Awake();
    }

    private void OnDisable()
    {
        _animationHandle?.Kill();
    }

    private Tween _animationHandle;
    public bool IsAnimating => _animationHandle != null && _animationHandle.active;

    public void BeginDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        gameObject.SetActive(true);

        SimpleView.SetAddress(ib.GetSimpleView().GetAddress());
        SimpleView.Refresh();

        ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
        {
            _mouseOffset = eventData.position;
            AnimateDisplay(extraBehaviourPivot.GetDisplayTransform(), extraBehaviourPivot.FollowTransform);
        }
    }

    private Vector2 _mouseOffset;

    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        bool dropOnNothing = !CanvasManager.Instance.RayCastIsHit(d);
        if (dropOnNothing)
        {
            ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
            if (extraBehaviourPivot != null)
                extraBehaviourPivot.AnimateState(SimpleView.GetDisplayTransform(), extraBehaviourPivot.IdleTransform);
        }
        
        gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
        {
            Drag(extraBehaviourPivot.FollowTransform, eventData.position);
        }
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.anchoredPosition = mouse - _mouseOffset;
        if (IsAnimating)
            return;
        SimpleView.SetDisplayTransform(pivot);
    }

    public RectTransform GetDisplayTransform()
        => SimpleView.GetDisplayTransform();







    private void SetDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        SimpleView.SetDisplayTransform(end);
    }

    private void AnimateDisplay(RectTransform start, RectTransform end)
    {
        SetDisplay(start);
        AnimateDisplay(end);
    }

    private void AnimateDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(SimpleView.GetDisplayTransform(), end);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }
}
