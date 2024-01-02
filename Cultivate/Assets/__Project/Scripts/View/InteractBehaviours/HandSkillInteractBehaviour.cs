
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandSkillInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler,
    IPointerClickHandler
{
    [SerializeField] private Image Image;
    [SerializeField] private CanvasGroup CanvasGroup;

    // public void BeginDrag(InteractBehaviour ib, PointerEventData eventData)
    // {
    //     CanvasManager.Instance.SkillAnnotation.SetAddressToNull(ib, eventData);
    //
    //     SetRaycastable(false);
    //     SetOpaque(false);
    //     CanvasManager.Instance.SkillGhost.BeginDrag(ComplexView.AddressBehaviour.GetAddress(),
    //         ComplexView.GetDisplayTransform(), ComplexView.PivotBehaviour.FollowPivot);
    // }
    //
    // public void EndDrag(InteractBehaviour ib, PointerEventData eventData)
    // {
    //     SetRaycastable(true);
    //     SetOpaque(true);
    //     ComplexView.AnimateBehaviour.SetStartAndPivot(CanvasManager.Instance.SkillGhost.AddressBehaviour.RectTransform, ComplexView.PivotBehaviour.IdlePivot);
    //     CanvasManager.Instance.SkillGhost.EndDrag();
    // }
    //
    // public void Drag(InteractBehaviour ib, PointerEventData eventData)
    // {
    //     CanvasManager.Instance.SkillGhost.Drag(ComplexView.PivotBehaviour.FollowPivot, eventData.position);
    // }

    public override void SetRaycastable(bool value)
    {
        Image.raycastTarget = value;
    }

    public override void SetOpaque(bool value)
    {
        CanvasGroup.alpha = value ? 1 : 0;
    }
}
