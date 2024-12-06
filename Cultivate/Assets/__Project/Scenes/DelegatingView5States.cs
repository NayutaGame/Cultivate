
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DelegatingView5States : DelegatingView
{
    protected override Animator InitAnimator()
    {
        // 0. hide
        // 1. idle
        // 2. hover
        // 3. follow
        // 4. free
        Animator animator = new(5, name);
        animator[-1, 0] = EnterHide;
        animator[-1, 1] = EnterIdle;
        animator[-1, 2] = EnterHover;
        animator[-1, 3] = EnterFollow;
        animator[-1, 4] = EnterFree;
        animator[4, -1] = ExitFree;
        return animator;
        
        // animator[-1, 4] = PingTween;
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

    [SerializeField] private Configuration HideConfiguration = new(localScale: Vector3.zero);
    [SerializeField] private Configuration IdleConfiguration = new(localScale: 0.5f * Vector3.one);
    [SerializeField] private Configuration HoverConfiguration = new(localPosition: 0.75f * Vector3.up, localScale: 0.75f * Vector3.one);
    [SerializeField] private Configuration FollowConfiguration = new(localScale: 0.75f * Vector3.one);

    private Tween EnterHide()
        => DOTween.Sequence()
            .AppendCallback(RecoverReparent)
            .Append(GoToConfiguration(HideConfiguration));

    private Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(RecoverReparent)
            .Append(GoToConfiguration(IdleConfiguration));

    private Tween EnterHover()
        => DOTween.Sequence()
            .AppendCallback(ReparentToAnchor)
            .Append(GoToConfiguration(HoverConfiguration));

    private Tween EnterFollow()
        => DOTween.Sequence()
            .AppendCallback(ReparentToAnchor)
            .Append(new FollowAnimation(GetDelegatedView().GetRect(), CanvasManager.Instance.GetPinAnchorRect()).GetHandle());

    private Tween EnterFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(false));

    private Tween ExitFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(true));

    private Tween GoToConfiguration(Configuration configuration)
    {
        return new GoToConfigurationAnimation(GetRect(), GetDelegatedView().GetRect(), configuration).GetHandle();
    }
    
    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(2);
    }
    
    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(1);
    }
    
    private void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(3);
    }
    
    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(1);
    }

    public void ReparentToAnchor()
    {
        GetDelegatedView().GetRect().SetParent(CanvasManager.Instance.GetPinAnchorRect());
    }

    public void RecoverReparent()
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
        Vector3 position = CanvasManager.Instance.UI2World(eventData.position);
        CanvasManager.Instance.GetPinAnchorRect().position = position;
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
