using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CommodityItemDelegate : InteractDelegate,
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler,
    IPointerClickHandler
{
    public PenetrateSkillView SkillView;
}
