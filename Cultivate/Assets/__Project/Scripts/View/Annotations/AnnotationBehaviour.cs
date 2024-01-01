
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationBehaviour : LegacyAddressBehaviour
{
    private void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        RectTransform.pivot = pivot;
        RectTransform.position = pos;
    }

    public void UpdateMousePos(InteractBehaviour ib, PointerEventData d)
        => UpdateMousePos(d.position);

    public void SetAddressFromIB(InteractBehaviour ib, PointerEventData d)
    {
        SetAddress(ib.ComplexView.AddressBehaviour.GetAddress());
    }

    public void SetAddressToNull(InteractBehaviour ib, PointerEventData d)
    {
        SetAddress(null);
    }
}
