
using UnityEngine;

public class XBehaviourHoverGhost : XBehaviour
{
    public string GhostAddress;
    private HoverGhostView Ghost;

    public override void Init(XView view)
    {
        base.Init(view);
        Ghost ??= new Address(GhostAddress).Get<HoverGhostView>();
        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;
        
        ib.PointerEnterNeuron.Join(Ghost.PointerEnter);
        ib.PointerExitNeuron.Join(Ghost.PointerExit);
        
        ib.BeginDragNeuron.Join(Ghost.BeginDrag);
        ib.DraggingExitNeuron.Join(Ghost.DraggingExit);
        // ib.DroppingNeuron.Join(Ghost.Dropping);

        ib.BeginDragNeuron.Join(Ghost.ResetJingJie);
        ib.PointerExitNeuron.Join(Ghost.ResetJingJie);
        ib.RightClickNeuron.Join(Ghost.NextJingJie);
    }

    public RectTransform GetDisplayTransform()
        => Ghost.GetDisplayTransform();
}
