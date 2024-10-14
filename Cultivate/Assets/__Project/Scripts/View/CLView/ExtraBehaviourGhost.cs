
using UnityEngine;

public class ExtraBehaviourGhost : ExtraBehaviour
{
    public string GhostAddress;
    private GhostView Ghost;

    public override void Init(CLView clView)
    {
        base.Init(clView);

        Ghost ??= new Address(GhostAddress).Get<GhostView>();

        InitInteractBehaviour();
    }

    private void InitInteractBehaviour()
    {
        InteractBehaviour ib = CLView.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.BeginDragNeuron.Join(Ghost.BeginDrag);
        ib.BeginDragNeuron.Join(CLView.SetHide);
        
        ib.EndDragNeuron.Join(Ghost.EndDrag);
        ib.EndDragNeuron.Join(CLView.SetShow);
        
        ib.DragNeuron.Join(Ghost.Drag);
    }

    public void Hide()
    {
        CLView.SetShow();
        Ghost.gameObject.SetActive(false);
    }

    public RectTransform GetDisplayTransform()
        => Ghost.GetDisplayTransform();
}
