
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DelegatingView4States : DelegatingView
{
    protected override Animator InitAnimator()
    {
        // 0. hide
        // 1. idle
        // 2. hover
        // 3. follow
        Animator animator = new(4, name);
        animator[-1, 0] = GoToHide;
        animator[-1, 1] = GoToIdle;
        animator[-1, 2] = GoToHover;
        animator[-1, 3] = GoToFollow;
        return animator;
        
        // // 0 for hide, 1 for idle, 2 for hover, 3 for follow, 4 for ping
        // animator[0, -1] = SetInteractable;
        // animator[-1, 4] = PingTween;
        // return animator;
    }

    protected override void InitInteractBehaviour(InteractBehaviour ib)
    {
        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
        ib.BeginDragNeuron.Join(BeginDrag);
        ib.EndDragNeuron.Join(EndDrag);
        ib.DragNeuron.Join(Drag);
        // ib.DraggingExitNeuron.Join(DraggingExit);
    }

    private static Configuration HideConfiguration = new(localScale: Vector3.zero);
    private static Configuration IdleConfiguration = new(localScale: 0.5f * Vector3.one);
    private static Configuration HoverConfiguration = new(localPosition: 160 * Vector3.up, 0.75f * Vector3.one);
    private static Configuration FollowConfiguration = new(localScale: 0.75f * Vector3.one);

    private Tween GoToHide()
        => GoToConfiguration(HideConfiguration);

    private Tween GoToIdle()
        => GoToConfiguration(IdleConfiguration);

    private Tween GoToHover()
        => GoToConfiguration(HoverConfiguration);

    private Tween GoToFollow()
    {
        return new FollowAnimation(GetDelegatedView().GetRect(), PinAnchor.Instance.GetRect()).GetHandle();
    }

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
    
    private void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        ReparentToAnchor();
        GetAnimator().SetStateAsync(3);
    }
    
    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        RecoverReparent();
        GetAnimator().SetStateAsync(1);
    }

    private void ReparentToAnchor()
    {
        GetDelegatedView().GetRect().SetParent(PinAnchor.Instance.GetRect());
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
    
    // public void Dropping(LegacyInteractBehaviour ib, PointerEventData d)
    // {
    //     ib.GetCLView().SetIdle(ib, d);
    //     gameObject.SetActive(false);
    // }
    
    private void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        PinAnchor.Instance.GetRect().position = eventData.position;
    }



    // public void PlayAppearAnimation()
    // {
    //     GetAnimator().SetState(0);
    //     GetAnimator().SetStateAsync(1);
    // }
    //
    // public void PlayDisappearAnimation()
    // {
    //     GetAnimator().SetStateAsync(0);
    // }
    //
    // public void Disappear()
    // {
    //     GetAnimator().SetState(0);
    // }
    //
    // public void RefreshPivots()
    //     => GetAnimator().SetStateAsync(1);
    //
    // private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    // {
    //     // if (_animator.State != 0)
    //     //     _animator.SetStateAsync(1);
    // }
    //
    // public void PlayPingAnimation()
    //     => GetAnimator().SetStateAsync(4);
    //
    // private Tween HideTween()
    //     => DOTween.Sequence()
    //         .AppendCallback(() => GetInteractBehaviour().SetInteractable(false))
    //         .Append(GetDelegatedView().GetRect().DOScale(0, 0.15f).SetEase(Ease.OutQuad));
    //
    // private Tween IdleTween()
    //     => DOTween.Sequence()
    //         .Append(new FollowAnimation(GetDelegatedView().GetRect(), IdleTransform).GetHandle());
    //         // .AppendCallback(() => CLView.GetInteractBehaviour().SetInteractable(true))
    //
    // private Tween SetInteractable()
    //     => DOTween.Sequence()
    //         .AppendCallback(() => GetInteractBehaviour().SetInteractable(true));
    //
    // private Tween HoverTween()
    //     => new FollowAnimation(GetDelegatedView().GetRect(), HoverTransform).GetHandle();
    //
    // private Tween FollowTween()
    //     => new FollowAnimation(GetDelegatedView().GetRect(), FollowTransform).GetHandle();
    //
    // private Tween PingTween()
    //     => GetDelegatedView().GetRect().DOScale(1.5f, 0.075f).SetEase(Ease.OutQuad).SetLoops(2, loopType: LoopType.Yoyo);
}
