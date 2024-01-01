
using UnityEngine.EventSystems;

public class RunFormationIconInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public void PointerEnter(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddress(ComplexView.AddressBehaviour.GetAddress());
        // StageManager.Instance.Pause();
    }

    public void PointerExit(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.SetAddress(null);
        // StageManager.Instance.Resume();
    }

    public void PointerMove(InteractBehaviour view, PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.FormationAnnotation.UpdateMousePos(eventData.position);
    }
}
