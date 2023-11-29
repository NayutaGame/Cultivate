
using UnityEngine.EventSystems;

public class StageFormationIconDelegate : InteractDelegate,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void PointerEnter(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddress(view.GetComponent<IAddress>().GetAddress());
        CanvasManager.Instance.FormationAnnotation.Refresh();
        StageManager.Instance.Pause();
    }

    public void PointerExit(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddress(null);
        CanvasManager.Instance.FormationAnnotation.Refresh();
        StageManager.Instance.Resume();
    }

    public void PointerMove(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.UpdateMousePos(eventData.position);
    }
}
