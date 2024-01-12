
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationView : SimpleView
{
    private void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform rectTransform = GetDisplayTransform();
        rectTransform.pivot = pivot;
        rectTransform.position = pos;
    }

    public void UpdateMousePos(InteractBehaviour ib, PointerEventData d)
        => UpdateMousePos(d.position);

    public void SetAddressFromIB(InteractBehaviour ib, PointerEventData d)
    {
        SetAddress(ib.GetSimpleView().GetAddress());
    }

    public void SetAddressToNull(InteractBehaviour ib, PointerEventData d)
    {
        SetAddress(null);
    }
}
