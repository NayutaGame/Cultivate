
using UnityEngine.EventSystems;

public class BrowserSkillDelegate : InteractDelegate,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void PointerEnter(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.SetAddress(GetComponent<IAddress>().GetAddress());
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void PointerExit(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        CanvasManager.Instance.SkillAnnotation.Refresh();
    }

    public void PointerMove(InteractDelegate view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }
}
