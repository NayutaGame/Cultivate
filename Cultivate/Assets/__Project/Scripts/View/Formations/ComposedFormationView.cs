
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComposedFormationView : DelegatingView
{
    public override void SetAddress(Address address)
    {
        Get<Formation>()?.PingNeuron.Remove(SetPing);
        base.SetAddress(address);
        Get<Formation>()?.PingNeuron.Add(SetPing);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        Get<Formation>()?.PingNeuron.Remove(SetPing);
    }

    private void SetPing()
        => GetAnimator().SetStateAsync(3);
    
    private Tween EnterPing()
        => DOTween.Sequence()
            .Append(GetDelegatedView().GetRect().DOScale(1.5f, 0.075f).SetEase(Ease.OutQuad))
            .AppendCallback(Refresh)
            .Append(GetDelegatedView().GetRect().DOScale(1f, 0.075f).SetEase(Ease.InQuad));
    
    
    
    
    
    
    
    
    
    
    protected override Animator InitAnimator()
    {
        // 0. hide
        // 1. idle
        // 2. hover
        // 3. ping
        Animator animator = new(4);
        animator[-1, 0] = EnterHide;
        animator[-1, 1] = EnterIdle;
        animator[-1, 2] = EnterHover;
        animator[-1, 3] = EnterPing;
        return animator;
    }

    protected override void InitInteractBehaviour(InteractBehaviour ib)
    {
        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
        ib.DraggingExitNeuron.Join(DraggingExit);
    }

    [SerializeField] private Configuration HideConfiguration = new(localScale: Vector3.zero);
    [SerializeField] private Configuration IdleConfiguration = new(localScale: 1f * Vector3.one);
    [SerializeField] private Configuration HoverConfiguration = new(localScale: 1.5f * Vector3.one);

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

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
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
