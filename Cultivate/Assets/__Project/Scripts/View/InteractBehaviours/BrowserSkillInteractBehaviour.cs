
using UnityEngine.EventSystems;

public class BrowserSkillInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void PointerEnter(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.SetAddress(AddressBehaviour.GetAddress());
    }

    public void PointerExit(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
    }

    public void PointerMove(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }
}
