
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
        CanvasManager.Instance.SkillAnnotation.SetAddress(GetSimpleView().GetAddress().Append(".Skill"));
    }
}
