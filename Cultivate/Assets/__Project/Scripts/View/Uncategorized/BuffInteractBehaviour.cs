
using UnityEngine.EventSystems;

public class BuffInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void PointerEnter(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.SetAddress(AddressBehaviour.GetAddress());
        StageManager.Instance.Pause();
    }

    public void PointerExit(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.SetAddress(null);
        StageManager.Instance.Resume();
    }

    public void PointerMove(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.UpdateMousePos(eventData.position);
    }
}
