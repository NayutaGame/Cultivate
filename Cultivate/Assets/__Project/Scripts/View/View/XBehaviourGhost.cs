
using UnityEngine;

public class XBehaviourGhost : XBehaviour
{
    public string GhostAddress;
    private GhostView Ghost;

    public override void AwakeFunction(XView view)
    {
        base.AwakeFunction(view);
        Ghost ??= new Address(GhostAddress).Get<GhostView>();
        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.BeginDragNeuron.Join(Ghost.BeginDrag);
        ib.EndDragNeuron.Join(Ghost.EndDrag);
        ib.DragNeuron.Join(Ghost.Drag);
        ib.DroppingNeuron.Join(Ghost.Dropping);
    }

    public RectTransform GetDisplayTransform()
        => Ghost.GetDisplayTransform();
}
