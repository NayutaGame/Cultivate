
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class RunSkillView : AbstractSkillView
{
    public override ISkillModel GetSkillModel()
        => RunManager.Get<ISkillModel>(GetIndexPath());

    // public override void OnPointerClick(PointerEventData eventData) { }
    // public override void OnBeginDrag(PointerEventData eventData) { }
    // public override void OnEndDrag(PointerEventData eventData) { }
    // public override void OnDrag(PointerEventData eventData) { }
    // public override void OnDrop(PointerEventData eventData) { }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForPreview(GetIndexPath());
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.SetIndexPathForPreview(null);
    }

    public override void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.dragging) return;
        RunCanvas.Instance.UpdateMousePosForPreview(eventData.position);
    }
}
