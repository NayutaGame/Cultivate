
using UnityEngine.EventSystems;

public class DiscoverSkillInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler
{
    public void HoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        AudioManager.Play("CardHover");

        ComplexView.AnimateBehaviour.SetPivot(ComplexView.PivotBehaviour.HoverPivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(ComplexView.AddressBehaviour.GetAddress());
    }

    public void UnhoverAnimation(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        ComplexView.AnimateBehaviour.SetPivot(ComplexView.PivotBehaviour.IdlePivot);

        CanvasManager.Instance.SkillAnnotation.SetAddress(null);
    }

    public void PointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;

        CanvasManager.Instance.SkillAnnotation.UpdateMousePos(eventData.position);
    }
}
