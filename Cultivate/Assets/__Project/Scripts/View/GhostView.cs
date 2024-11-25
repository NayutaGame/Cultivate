
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostView : MonoBehaviour
{
    public SimpleView SimpleView;

    public void Awake()
    {
        SimpleView ??= GetComponent<SimpleView>();
        SimpleView.AwakeFunction();
    }

    private void OnDisable()
    {
        _animationHandle?.Kill();
    }

    private Tween _animationHandle;
    public bool IsAnimating => _animationHandle != null && _animationHandle.active;

    public void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        ib.GetCLView().SetInactive(ib, d);
        
        gameObject.SetActive(true);

        SimpleView.SetAddress(ib.GetSimpleView().GetAddress());
        SimpleView.Refresh();

        PivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<PivotBehaviour>();
        if (pivotBehaviour != null)
        {
            _mouseOffset = d.position;
            AnimateDisplay(pivotBehaviour.GetDisplayTransform(), pivotBehaviour.FollowTransform);
        }
    }

    private Vector2 _mouseOffset;

    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        ib.GetCLView().SetIdle(ib, d);

        // InteractBehaviour firstHit = CanvasManager.Instance.FirstRayCastHit(d);
        //
        // bool dropOnNothing = firstHit == null;
        // bool dropOnSelf = firstHit == ib;
        // if (dropOnNothing || dropOnSelf)
        // {
        //     ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        //     if (extraBehaviourPivot != null)
        //         extraBehaviourPivot.RectTransformToIdle(SimpleView.GetDisplayTransform());
        // }
        
        PivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<PivotBehaviour>();
        if (pivotBehaviour != null)
            pivotBehaviour.RectTransformToIdle(SimpleView.GetViewTransform());
        
        gameObject.SetActive(false);
    }

    public void Dropping(InteractBehaviour ib, PointerEventData d)
    {
        ib.GetCLView().SetIdle(ib, d);
        gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        PivotBehaviour pivotBehaviour = ib.GetCLView().GetBehaviour<PivotBehaviour>();
        if (pivotBehaviour != null)
        {
            Drag(pivotBehaviour.FollowTransform, eventData.position);
        }
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = CanvasManager.Instance.UI2World(mouse);
        if (IsAnimating)
            return;
        SimpleView.SetViewTransform(pivot);
    }

    public RectTransform GetDisplayTransform()
        => SimpleView.GetViewTransform();







    private void SetDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        SimpleView.SetViewTransform(end);
    }

    private void AnimateDisplay(RectTransform start, RectTransform end)
    {
        SetDisplay(start);
        AnimateDisplay(end);
    }

    private void AnimateDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(SimpleView.GetViewTransform(), end);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }
}
