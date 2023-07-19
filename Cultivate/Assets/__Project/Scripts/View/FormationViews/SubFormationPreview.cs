using UnityEngine;
using UnityEngine.EventSystems;

public class SubFormationPreview : SubFormationView
{
    public SubFormationView SubFormationView;

    public override void Configure(IndexPath indexPath)
    {
        base.Configure(indexPath);
        if (SubFormationView != null)
            SubFormationView.Configure(indexPath);
    }

    public override void Refresh()
    {
        if (GetIndexPath() == null)
        {
            gameObject.SetActive(false);
            return;
        }
        base.Refresh();
        if (SubFormationView != null)
            SubFormationView.Refresh();
    }

    public void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        _rectTransform.pivot = pivot;
        _rectTransform.position = pos;
    }

    // public override void OnPointerClick(PointerEventData eventData) { }
    // public override void OnBeginDrag(PointerEventData eventData) { }
    // public override void OnEndDrag(PointerEventData eventData) { }
    // public override void OnDrag(PointerEventData eventData) { }
    // public override void OnDrop(PointerEventData eventData) { }

    public override void OnPointerEnter(PointerEventData eventData) { }
    public override void OnPointerExit(PointerEventData eventData) { }
    public override void OnPointerMove(PointerEventData eventData) { }
}
