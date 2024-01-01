
using UnityEngine.EventSystems;

public class StageSkillInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void PointerEnter(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.SetAddress(GetComponent<LegacyAddressBehaviour>().GetAddress());
        StageManager.Instance.Pause();
    }

    public void PointerExit(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
        StageManager.Instance.Resume();
    }

    public void PointerMove(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;
        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }
}
