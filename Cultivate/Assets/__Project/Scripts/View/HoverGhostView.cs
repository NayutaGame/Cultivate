
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverGhostView : MonoBehaviour
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

    public void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetHide(ib, d);
        ib.GetCLView().SetVisible(false);
        
        gameObject.SetActive(true);

        SimpleView.SetAddress(ib.GetSimpleView().GetAddress());
        SimpleView.Refresh();

        ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
        {
            AnimateDisplay(extraBehaviourPivot.GetDisplayTransform(), extraBehaviourPivot.HoverTransform);
        }
    }

    public void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetShow(ib, d);
        ib.GetCLView().SetVisible(true);
        
        ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
            extraBehaviourPivot.RectTransformToIdle(SimpleView.GetDisplayTransform());
        
        gameObject.SetActive(false);
    }

    public void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        gameObject.SetActive(false);
    }

    public void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        to.GetCLView().SetVisible(true);
    }

    public void Dropping(InteractBehaviour ib, PointerEventData d)
    {
        // ib.GetCLView().SetShow(ib, d);
        // gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        // ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        // if (extraBehaviourPivot != null)
        // {
        //     Drag(extraBehaviourPivot.FollowTransform, eventData.position);
        // }
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = CanvasManager.Instance.UI2World(mouse);
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
