
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DeckSkillInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler,
    IPointerClickHandler
{
    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        SetPivot(PivotBehaviour.HoverPivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(GetSkillAddress());
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        SetPivot(PivotBehaviour.IdlePivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }

    public void BeginDrag(PointerEventData eventData)
    {
        CanvasManager.Instance.SkillAnnotation.SetAddress(null);

        SetRaycastable(false);
        SetOpaque(false);
        CanvasManager.Instance.SkillGhost.BeginDrag(GetSkillAddress(), AddressBehaviour.RectTransform, PivotBehaviour.FollowPivot);
    }

    public void EndDrag(PointerEventData eventData)
    {
        SetRaycastable(true);
        SetOpaque(true);
        SetStartAndPivot(CanvasManager.Instance.SkillGhost.AddressBehaviour.RectTransform, PivotBehaviour.IdlePivot);
        CanvasManager.Instance.SkillGhost.EndDrag();
    }

    public void Drag(PointerEventData eventData)
    {
        CanvasManager.Instance.SkillGhost.Drag(PivotBehaviour.FollowPivot, eventData.position);
    }

    protected abstract Address GetSkillAddress();
}
