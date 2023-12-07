
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckSkillInteractBehaviour : InteractBehaviour,
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

        CanvasManager.Instance.SkillAnnotation.SetAddress(AddressBehaviour.GetAddress());
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
        CanvasManager.Instance.SkillGhost.BeginDrag(AddressBehaviour.GetAddress(), AddressBehaviour.RectTransform, PivotBehaviour.FollowPivot);

        AddressBehaviour.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void EndDrag(PointerEventData eventData)
    {
        SetStartAndPivot(CanvasManager.Instance.SkillGhost.AddressBehaviour.RectTransform, PivotBehaviour.IdlePivot);
        CanvasManager.Instance.SkillGhost.EndDrag();

        AddressBehaviour.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void Drag(PointerEventData eventData)
    {
        CanvasManager.Instance.SkillGhost.Drag(PivotBehaviour.FollowPivot, eventData.position);
    }
}
