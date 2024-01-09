
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostView : MonoBehaviour
{
    [SerializeField] private AddressBehaviour AddressBehaviour;

    private Tween _animationHandle;
    public bool IsAnimating => _animationHandle != null && _animationHandle.active;

    public void BeginDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        gameObject.SetActive(true);
        AnimateDisplay(ib.ComplexView.GetDisplayTransform(), ib.ComplexView.GetPivotBehaviour().FollowPivot);
    }

    public void EndDrag(InteractBehaviour ib, PointerEventData eventData)
    {
        ib.ComplexView.GetAnimateBehaviour().AnimateDisplay(AddressBehaviour.Base, ib.ComplexView.GetPivotBehaviour().IdlePivot);
        gameObject.SetActive(false);
    }

    public void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        Drag(ib.ComplexView.GetPivotBehaviour().FollowPivot, eventData.position);
    }

    private void Drag(RectTransform pivot, Vector2 mouse)
    {
        pivot.position = mouse;
        if (IsAnimating)
            return;
        AddressBehaviour.Base.position = pivot.position;
        AddressBehaviour.Base.localScale = pivot.localScale;
    }






    public void SetAddressFromIB(AddressBehaviour ab, PointerEventData d)
    {
        AddressBehaviour.SetAddress(ab.GetAddress());
    }

    public void SetAddressToNull(AddressBehaviour ab, PointerEventData d)
    {
        AddressBehaviour.SetAddress(null);
    }

    private void SetDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        AddressBehaviour.Base.position = end.position;
        AddressBehaviour.Base.localScale = end.localScale;
    }

    private void AnimateDisplay(RectTransform start, RectTransform end)
    {
        SetDisplay(start);
        AnimateDisplay(end);
    }

    private void AnimateDisplay(RectTransform end)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(AddressBehaviour.Base, end);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }
}
