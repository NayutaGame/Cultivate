using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunCardPreview : AbstractCardView
{
    public RunCardView CardView;

    public override ICardModel GetCardModel()
        => RunManager.Get<ICardModel>(GetIndexPath());

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);
        CardView.Configure(indexPath);
    }

    public override void Refresh()
    {
        base.Refresh();
        CardView.Refresh();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }


    public override void OnPointerDown(PointerEventData eventData) { }
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnDrop(PointerEventData eventData) { }

    public override void OnPointerEnter(PointerEventData eventData) { }
    public override void OnPointerExit(PointerEventData eventData) { }
    public override void OnPointerMove(PointerEventData eventData) { }
}
