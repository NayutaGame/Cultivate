
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DelegatingView3States : DelegatingView
{
    protected override Animator InitAnimator()
    {
        // 0. hide
        // 1. idle
        // 2. hover
        Animator animator = new(3);
        animator[-1, 0] = GoToHide;
        animator[-1, 1] = GoToIdle;
        animator[-1, 2] = GoToHover;
        return animator;
        
        // // 0 for hide, 1 for idle, 2 for hover, 3 for follow, 4 for ping
        // Animator animator = new(5, View.name);
        // animator[-1, 0] = HideTween;
        // animator[0, -1] = SetInteractable;
        // animator[-1, 1] = IdleTween;
        // animator[-1, 2] = HoverTween;
        // animator[-1, 3] = FollowTween;
        // animator[-1, 4] = PingTween;
        // return animator;
    }

    protected override void InitInteractBehaviour(InteractBehaviour ib)
    {
        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
        ib.DraggingExitNeuron.Join(DraggingExit);
    }

    private static Configuration HideConfiguration = new(localScale: Vector3.zero);
    private static Configuration IdleConfiguration = new(localScale: 0.5f * Vector3.one);
    private static Configuration HoverConfiguration = new(localScale: 0.75f * Vector3.one);

    private Tween GoToHide()
        => GoToConfiguration(HideConfiguration);

    private Tween GoToIdle()
        => GoToConfiguration(IdleConfiguration);

    private Tween GoToHover()
        => GoToConfiguration(HoverConfiguration);

    private Tween GoToConfiguration(Configuration configuration)
    {
        return new GoToConfigurationAnimation(GetRect(), GetDelegatedView().GetRect(), configuration).GetHandle();
        //         .AppendCallback(() => GetInteractBehaviour().SetInteractable(false))
        //         .Append(GetDelegatedView().GetRect().DOScale(0, 0.15f).SetEase(Ease.OutQuad));
    }
    
    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        ReparentToAnchor();
        GetAnimator().SetStateAsync(2);
    }
    
    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        RecoverReparent();
        // if (GetAnimator().State != 0)
        GetAnimator().SetStateAsync(1);
    }

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        RecoverReparent();
        // if (GetAnimator().State != 0)
        GetAnimator().SetStateAsync(1);
    }

    private void ReparentToAnchor()
    {
        GetDelegatedView().GetRect().SetParent(CanvasManager.Instance.GetPinAnchorRect());
    }

    private void RecoverReparent()
    {
        AnimatedListView parent = GetParentListView();
        if (parent != null)
        {
            parent.RecoverDelegatingView(this);
        }
        else
        {
            GetDelegatedView().GetRect().SetParent(GetRect());
        }
    }
}
