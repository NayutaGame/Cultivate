
using UnityEngine;
using UnityEngine.EventSystems;

public class AnnotationView : AddressBehaviour
{
    private void UpdateMousePos(Vector2 pos)
    {
        Vector2 pivot = new Vector2(Mathf.RoundToInt(pos.x / Screen.width), Mathf.RoundToInt(pos.y / Screen.height));
        Base.pivot = pivot;
        Base.position = pos;
    }

    public void UpdateMousePos(InteractBehaviour ib, PointerEventData d)
        => UpdateMousePos(d.position);

    public void SetAddressFromIB(InteractBehaviour ib, PointerEventData d)
    {
        SetAddress(ib.ComplexView.GetAddress());
    }

    public void SetAddressToNull(MonoBehaviour behaviour, PointerEventData d)
    {
        SetAddress(null);
    }
}
