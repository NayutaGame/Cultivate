
using UnityEngine;

public class LegacyGhostBehaviour : LegacyBehaviour
{
    public string GhostAddress;
    private GhostView Ghost;

    public override void Init(LegacyView view)
    {
        base.Init(view);
        Ghost ??= new Address(GhostAddress).Get<GhostView>();
        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        LegacyInteractBehaviour ib = View.GetInteractBehaviour();
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
