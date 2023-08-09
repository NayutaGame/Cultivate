
using UnityEngine;
using UnityEngine.EventSystems;

public class RunSkillPreview : SkillView
{
    public RunSkillView SkillView;

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

    public override void OnPointerClick(PointerEventData eventData) { }
    public override void OnBeginDrag(PointerEventData eventData) { }
    public override void OnEndDrag(PointerEventData eventData) { }
    public override void OnDrag(PointerEventData eventData) { }
    public override void OnDrop(PointerEventData eventData) { }

    public override void OnPointerEnter(PointerEventData eventData) { }
    public override void OnPointerExit(PointerEventData eventData) { }
    public override void OnPointerMove(PointerEventData eventData) { }
}
