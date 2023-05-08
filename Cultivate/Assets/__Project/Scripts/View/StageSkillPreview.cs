using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageSkillPreview : AbstractSkillView
{
    public StageSkillView SkillView;

    public override ISkillModel GetSkillModel()
        => StageManager.Get<ISkillModel>(GetIndexPath());

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);
        SkillView.Configure(indexPath);
    }

    public override void Refresh()
    {
        base.Refresh();
        SkillView.Refresh();
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
