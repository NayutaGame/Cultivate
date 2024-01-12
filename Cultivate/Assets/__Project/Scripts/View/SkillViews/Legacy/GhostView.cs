
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostView : MonoBehaviour
{
    [SerializeField] private SimpleView SimpleView;

    private Tween _animationHandle;
    public bool IsAnimating => _animationHandle != null && _animationHandle.active;

    public void BeginDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        gameObject.SetActive(true);

        StateBehaviourPivot stateBehaviourPivot = ib.GetCLView().GetStateBehaviour() as StateBehaviourPivot;
        if (stateBehaviourPivot != null)
            AnimateDisplay(stateBehaviourPivot.BaseTransform, stateBehaviourPivot.FollowTransform);
    }

    public void EndDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        StateBehaviourPivot stateBehaviourPivot = ib.GetCLView().GetStateBehaviour() as StateBehaviourPivot;
        if (stateBehaviourPivot != null)
            stateBehaviourPivot.AnimateState(SimpleView.GetDisplayTransform(), stateBehaviourPivot.IdleTransform);

        gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        StateBehaviourPivot stateBehaviourPivot = ib.GetCLView().GetStateBehaviour() as StateBehaviourPivot;
        if (stateBehaviourPivot != null)
            Drag(stateBehaviourPivot.FollowTransform, eventData.position);
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = mouse;
        if (IsAnimating)
            return;
        SimpleView.GetDisplayTransform().position = pivot.position;
        SimpleView.GetDisplayTransform().localScale = pivot.localScale;
    }






    public void SetAddressFromIB(InteractBehaviour ib, PointerEventData d)
    {
        SimpleView.SetAddress(ib.GetSimpleView().GetAddress());
    }

    public void SetAddressToNull(InteractBehaviour ib, PointerEventData d)
    {
        SimpleView.SetAddress(null);
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
