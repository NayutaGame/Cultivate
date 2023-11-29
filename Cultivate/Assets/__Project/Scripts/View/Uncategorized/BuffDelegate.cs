
using UnityEngine.EventSystems;

public class BuffDelegate : InteractDelegate,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void PointerEnter(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.SetAddress(GetComponent<IAddress>().GetAddress());
        CanvasManager.Instance.BuffAnnotation.Refresh();
        StageManager.Instance.Pause();
    }

    public void PointerExit(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.SetAddress(null);
        CanvasManager.Instance.BuffAnnotation.Refresh();
        StageManager.Instance.Resume();
    }

    public void PointerMove(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.BuffAnnotation.UpdateMousePos(eventData.position);
    }
}
