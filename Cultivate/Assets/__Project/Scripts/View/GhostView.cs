
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostView : MonoBehaviour
{
    public XView SimpleView;

    public void Awake()
    {
        SimpleView ??= GetComponent<XView>();
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
        ib.GetView().SetHide(ib, d);
        
        gameObject.SetActive(true);

        SimpleView.SetAddress(ib.GetAddress());
        SimpleView.Refresh();

        XBehaviourPivot xBehaviourPivot = ib.GetBehaviour<XBehaviourPivot>();
        if (xBehaviourPivot != null)
        {
            _mouseOffset = d.position;
            AnimateDisplay(xBehaviourPivot.GetDisplayTransform(), xBehaviourPivot.FollowTransform);
        }
    }

    private Vector2 _mouseOffset;

    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        ib.GetView().SetShow(ib, d);

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
        
        XBehaviourPivot xBehaviourPivot = ib.GetBehaviour<XBehaviourPivot>();
        if (xBehaviourPivot != null)
            xBehaviourPivot.RectTransformToIdle(SimpleView.RectTransform);
        
        gameObject.SetActive(false);
    }

    public void Dropping(InteractBehaviour ib, PointerEventData d)
    {
        ib.GetView().SetShow(ib, d);
        gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        XBehaviourPivot xBehaviourPivot = ib.GetBehaviour<XBehaviourPivot>();
        if (xBehaviourPivot != null)
        {
            Drag(xBehaviourPivot.FollowTransform, eventData.position);
        }
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = CanvasManager.Instance.UI2World(mouse);
        if (IsAnimating)
            return;
        
        SimpleView.RectTransform.position = pivot.position;
        SimpleView.RectTransform.localScale = pivot.localScale;
    }

    public RectTransform GetDisplayTransform()
        => SimpleView.RectTransform;







    private void SetDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        SimpleView.RectTransform.position = end.position;
        SimpleView.RectTransform.localScale = end.localScale;
    }

    private void AnimateDisplay(RectTransform start, RectTransform end)
    {
        SetDisplay(start);
        AnimateDisplay(end);
    }

    private void AnimateDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(SimpleView.RectTransform, end);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }
}
