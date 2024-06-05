
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

        ib.BeginDragNeuron.Join(Ghost.BeginDrag);
        ib.BeginDragNeuron.Join(CLView.SetInteractableToFalse);
        ib.BeginDragNeuron.Join(CLView.SetVisibleToFalse);
        
        ib.EndDragNeuron.Join(Ghost.EndDrag);
        ib.EndDragNeuron.Join(CLView.SetInteractableToTrue);
        ib.EndDragNeuron.Join(CLView.SetVisibleToTrue);
        
        ib.DragNeuron.Join(Ghost.Drag);
    }

    public void Hide()
    {
        CLView.SetInteractableToTrue();
        CLView.SetVisibleToTrue();
        Ghost.gameObject.SetActive(false);
    }

    public RectTransform GetDisplayTransform()
        => Ghost.GetDisplayTransform();
}
