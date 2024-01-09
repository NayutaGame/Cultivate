
using UnityEngine.EventSystems;

public abstract class DragStrategy
{
    public abstract void BeginDrag(AddressBehaviour ab, PointerEventData d);
    public abstract void EndDrag(AddressBehaviour ab, PointerEventData d);
    public abstract void Drag(AddressBehaviour ab, PointerEventData d);
}
