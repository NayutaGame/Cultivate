
using UnityEngine;

public class ExtraBehaviourGhost : ExtraBehaviour
{
    public string GhostAddress;
    private GhostView Ghost;
    // private Animator _animator;

    public override void Init(CLView clView)
    {
        base.Init(clView);

        Ghost = new Address(GhostAddress).Get<GhostView>();

        InitInteractBehaviour();
        // _animator ??= InitAnimator();
    }

    // private Animator InitAnimator()
    // {
    //     // 0 for hide, 1 for show
    //     Animator animator = new(2);
    //     animator[-1, 1] = () => TweenAnimation.TweenFromAction(Hide);
    //     animator[-1, 0] = HideTween;
    //     return animator;
    // }

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
