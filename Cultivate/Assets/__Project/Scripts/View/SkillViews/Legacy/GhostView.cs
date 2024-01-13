
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostView : MonoBehaviour
{
    private SimpleView SimpleView;

    public void Awake()
    {
        SimpleView ??= GetComponent<SimpleView>();
        SimpleView.Awake();
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
            AnimateDisplay(extraBehaviourPivot.GetDisplayTransform(), extraBehaviourPivot.FollowTransform);
    }

    public void EndDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
            extraBehaviourPivot.AnimateState(SimpleView.GetDisplayTransform(), extraBehaviourPivot.IdleTransform);

        gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        ExtraBehaviourPivot extraBehaviourPivot = ib.GetCLView().GetExtraBehaviour<ExtraBehaviourPivot>();
        if (extraBehaviourPivot != null)
            Drag(extraBehaviourPivot.FollowTransform, eventData.position);
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = mouse;
        if (IsAnimating)
            return;
        SimpleView.GetDisplayTransform().position = pivot.position;
        SimpleView.GetDisplayTransform().localScale = pivot.localScale;
    }







    private void SetDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        SimpleView.GetDisplayTransform().position = end.position;
        SimpleView.GetDisplayTransform().localScale = end.localScale;
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
