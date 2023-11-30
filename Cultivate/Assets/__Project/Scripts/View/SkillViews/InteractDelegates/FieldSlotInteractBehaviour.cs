
using UnityEngine.EventSystems;

public class FieldSlotInteractBehaviour : InteractBehaviour
{
    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        SetPivot(PivotBehaviour.HoverPivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(AddressBehaviour.GetAddress());
        // CanvasManager.Instance.SkillAnnotation.SetAddress(SkillView.GetAddress());
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
        eventData.pointerDrag = gameObject;

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);

        // AddressBehaviour.CanvasGroup.alpha = 0;

        CanvasManager.Instance.SkillGhost.RectTransform.position = AddressBehaviour.RectTransform.position;

        CanvasManager.Instance.SkillGhost.CanvasGroup.alpha = 1;
        CanvasManager.Instance.SkillGhost.SetAddress(AddressBehaviour.GetAddress());
        CanvasManager.Instance.SkillGhost.Refresh();
        CanvasManager.Instance.SkillGhost.SetPivot(PivotBehaviour.FollowPivot);

        SetEnabled(false);
    }

    public void EndDrag(PointerEventData eventData)
    {
        // AddressBehaviour.CanvasGroup.alpha = 1;
        AddressBehaviour.RectTransform.position = CanvasManager.Instance.SkillGhost.RectTransform.position;

        CanvasManager.Instance.SkillGhost.CanvasGroup.alpha = 0;
        CanvasManager.Instance.SkillGhost.SetAddress(null);
        CanvasManager.Instance.SkillGhost.Refresh();

        SetPivot(PivotBehaviour.IdlePivot);

        SetEnabled(true);
    }

    public void Drag(PointerEventData eventData)
    {
        PivotBehaviour.FollowPivot.position = eventData.position;
        if (CanvasManager.Instance.SkillGhost.IsAnimating)
            return;
        CanvasManager.Instance.SkillGhost.SetPivotWithoutAnimation(PivotBehaviour.FollowPivot);
    }
}
