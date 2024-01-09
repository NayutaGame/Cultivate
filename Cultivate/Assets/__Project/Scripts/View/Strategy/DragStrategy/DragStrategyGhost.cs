
using UnityEngine.EventSystems;

public class DragStrategyGhost : DragStrategy
{
    public GhostView Ghost;

    public DragStrategyGhost(GhostView ghost)
    {
        Ghost = ghost;
    }

    public override void BeginDrag(AddressBehaviour ab, PointerEventData d)
    {
        Ghost.SetAddressFromIB(ab, d);
        Ghost.BeginDrag(ab, d);
    }

    public override void EndDrag(AddressBehaviour ab, PointerEventData d)
    {
        Ghost.EndDrag(ab, d);
    }

    public override void Drag(AddressBehaviour ab, PointerEventData d)
    {
        Ghost.Drag(ab, d);
    }
}
