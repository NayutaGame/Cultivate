
using DG.Tweening;
using UnityEngine.EventSystems;

public class RunCardView : AbstractCardView
{
    public override ICardModel GetCardModel()
        => RunManager.Get<ICardModel>(GetIndexPath());

    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnDrop(PointerEventData eventData) { }

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
