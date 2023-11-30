
using UnityEngine.EventSystems;

public class CommodityItemInteractBehaviour : InteractBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler
{
    public PenetrateSkillView SkillView;
}
