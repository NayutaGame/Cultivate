
using UnityEngine;

public class ExtraBehaviourHoverGhost : ExtraBehaviour
{
    public string GhostAddress;
    private HoverGhostView Ghost;

    public override void Init(CLView clView)
    {
        base.Init(clView);
        Ghost ??= new Address(GhostAddress).Get<HoverGhostView>();
        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = CLView.GetInteractBehaviour();
        if (ib == null)
            return;
        
        ib.PointerEnterNeuron.Join(Ghost.PointerEnter);
        ib.PointerExitNeuron.Join(Ghost.PointerExit);
        
        ib.BeginDragNeuron.Join(Ghost.BeginDrag);
        ib.DraggingExitNeuron.Join(Ghost.DraggingExit);
        // ib.DroppingNeuron.Join(Ghost.Dropping);
    }

    public RectTransform GetDisplayTransform()
        => Ghost.GetDisplayTransform();
}
