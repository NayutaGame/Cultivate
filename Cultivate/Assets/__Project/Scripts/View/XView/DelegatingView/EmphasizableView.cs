
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmphasizableView : DelegatingView
{
    [Header("Emphasis Animation")]
    [SerializeField] private float _emphasizedScale = 1.5f;  // 强调时的缩放
    [SerializeField] private float _emphasisDuration = 0.075f;  // 强调动画时长

    public override void SetAddress(Address address)
    {
        Get<IEmphasizable>()?.GetEmphasisNeuron().Remove(SetPing);
        base.SetAddress(address);
        Get<IEmphasizable>()?.GetEmphasisNeuron().Add(SetPing);
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        // TODO: 이거 왜 오류나는지 모르겠음
        // Get<IEmphasizable>()?.GetEmphasisNeuron().Remove(SetPing);
        GrabberRelease();
    }

    // private void SetPing()
    //     => GetAnimator().SetStateAsync(3);

    private void SetPing()
    {
        if (isActiveAndEnabled)
        {
            GetAnimator().SetStateAsync(3);
        }
    }
    
    private Tween EnterPing()
        => DOTween.Sequence()
            .AppendCallback(Refresh)
            .Append(GetDelegatedView().GetRect().DOScale(_emphasizedScale, _emphasisDuration).SetEase(Ease.OutQuad))
            .Append(GetDelegatedView().GetRect().DOScale(1f, _emphasisDuration).SetEase(Ease.InQuad));
    
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
            .AppendCallback(GrabberRelease)
            .Append(GoToConfiguration(HideConfiguration));

    private Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(GoToConfiguration(IdleConfiguration));

    private Tween EnterHover()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetHover)
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

    private void GrabberSetHover()
    {
        CanvasManager.Instance.GetGrabber().SetHover(this);
    }

    private void GrabberRelease()
    {
        CanvasManager.Instance.GetGrabber().Release(this);
    }
}
