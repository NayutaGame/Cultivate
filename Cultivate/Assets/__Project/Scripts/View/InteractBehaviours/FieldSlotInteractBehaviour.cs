
using UnityEngine.EventSystems;

public class FieldSlotInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IBeginDragHandler, IEndDragHandler, IDragHandler,
    IDropHandler,
    IPointerClickHandler
{
    public void HoverAnimation(InteractBehaviour ib, PointerEventData eventData)
    {
        AudioManager.Play("CardHover");
        CanvasManager.Instance.SkillAnnotation.SetAddress(GetSkillAddress());
    }

    // public void BeginDrag(InteractBehaviour ib, PointerEventData eventData)
    // {
    //     CanvasManager.Instance.SkillAnnotation.SetAddress(null);
    //
    //     SetRaycastable(false);
    //     SetOpaque(false);
    //     CanvasManager.Instance.SkillGhost.BeginDrag(GetSkillAddress(),
    //         ComplexView.AddressBehaviour.RectTransform, ComplexView.PivotBehaviour.FollowPivot);
    // }
    //
    // public void EndDrag(InteractBehaviour ib, PointerEventData eventData)
    // {
    //     SetRaycastable(true);
    //     SetOpaque(true);
    //     ComplexView.AnimateBehaviour.SetStartAndPivot(CanvasManager.Instance.SkillGhost.AddressBehaviour.RectTransform, ComplexView.PivotBehaviour.IdlePivot);
    //     CanvasManager.Instance.SkillGhost.EndDrag();
    // }
    //
    // public void Drag(InteractBehaviour ib, PointerEventData eventData)
    // {
    //     CanvasManager.Instance.SkillGhost.Drag(ComplexView.PivotBehaviour.FollowPivot, eventData.position);
    // }

    protected Address GetSkillAddress()
        => ComplexView.AddressBehaviour.GetAddress().Append(".Skill");
}
