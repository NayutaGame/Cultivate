
using UnityEngine;

public class ExtraBehaviourGhost : ExtraBehaviour
{
    public string GhostAddress;
    private GhostView Ghost;

    public override void Init(CLView clView)
    {
        base.Init(clView);

        Ghost = new Address(GhostAddress).Get<GhostView>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = CLView.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.BeginDragNeuron.Add(Ghost.BeginDrag);
        ib.EndDragNeuron.Add(Ghost.EndDrag);
        ib.DragNeuron.Add(Ghost.Drag);

        ib.BeginDragNeuron.Add(CLView.SetInteractableToFalse);
        ib.BeginDragNeuron.Add(CLView.SetVisibleToFalse);
        ib.EndDragNeuron.Add(CLView.SetInteractableToTrue);
        ib.EndDragNeuron.Add(CLView.SetVisibleToTrue);
    }

    public void FromDrop()
    {
        CLView.SetInteractableToTrue();
        CLView.SetVisibleToTrue();
        Ghost.FromDrop();
    }

    public RectTransform GetDisplayTransform()
        => Ghost.GetDisplayTransform();
}
