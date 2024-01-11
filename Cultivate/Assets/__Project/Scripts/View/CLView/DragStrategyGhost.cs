
using UnityEngine.EventSystems;

public class DragStrategyGhost
{
    public GhostView Ghost;

    public DragStrategyGhost(GhostView ghost)
    {
        Ghost = ghost;
    }

    public void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        Ghost.SetAddressFromIB(ib, d);
        Ghost.BeginDrag(ib, d);
    }

    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        Ghost.EndDrag(ib, d);
    }

    public void Drag(InteractBehaviour ib, PointerEventData d)
    {
        Ghost.Drag(ib, d);
    }
}
