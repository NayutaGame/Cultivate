
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmphasizableSlotView : ScaleSlotView
{
    [Header("Emphasis Animation")]
    [SerializeField] private float _emphasizedScale = 1.5f;
    [SerializeField] private float _emphasisDuration = 0.075f;

    public override void SetAddress(Address address)
    {
        Get<IEmphasizable>()?.GetEmphasisNeuron().Remove(SetPing);
        base.SetAddress(address);
        Get<IEmphasizable>()?.GetEmphasisNeuron().Add(SetPing);
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
            GetAnimator().SetStateAsync(PING);
        }
    }
    
    private Tween EnterPing()
        => DOTween.Sequence()
            .AppendCallback(Refresh)
            .Append(GetContentView().GetRect().DOScale(_emphasizedScale, _emphasisDuration).SetEase(Ease.OutQuad))
            .Append(GetContentView().GetRect().DOScale(1f, _emphasisDuration).SetEase(Ease.InQuad));


    private new static readonly int ANY = -1;
    private new static readonly int FREE = 0;
    private new static readonly int IDLE = 1;
    private new static readonly int HOVER = 2;
    private new static readonly int FOLLOW = 3;
    private static readonly int PING = 4;
    
    protected override Animator InitAnimator()
    {
        Animator animator = new(5, name);
        animator[ANY, IDLE] = EnterIdle;
        animator[ANY, HOVER] = EnterHover;
        animator[ANY, FOLLOW] = EnterFollow;
        animator[ANY, PING] = EnterPing;
        animator[ANY, FREE] = EnterFree;
        animator[FREE, ANY] = ExitFree;
        return animator;
    }

    public override void SetInteractBehaviour(InteractBehaviour ib)
    {
        if (_interactBehaviour != null)
        {
            ib.PointerEnterNeuron.Remove(PointerEnter);
            ib.PointerExitNeuron.Remove(PointerExit);
            ib.DraggingExitNeuron.Remove(DraggingExit);
        }
        base.SetInteractBehaviour(ib);
        if (_interactBehaviour != null)
        {
            ib.PointerEnterNeuron.Join(PointerEnter);
            ib.PointerExitNeuron.Join(PointerExit);
            ib.DraggingExitNeuron.Join(DraggingExit);
        }
    }

    private Tween EnterIdle()
        => DOTween.Sequence()
            .AppendCallback(GrabberRelease)
            .Append(GoToConfiguration(IdleConfiguration));

    private Tween EnterHover()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetHover)
            .Append(GoToConfiguration(HoverConfiguration));

    private Tween EnterFollow()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetDrag)
            .Append(FollowAnimation.FromDefault(GetContentView().GetRect(), CanvasManager.Instance.GetGrabber().GetRect()).GetHandle());

    private Tween EnterFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(false));

    private Tween ExitFree()
        => DOTween.Sequence()
            .AppendCallback(() => GetInteractBehaviour().SetInteractable(true));

    private Tween GoToConfiguration(Configuration configuration)
    {
        return FollowAnimation.FromConfiguration(GetContentView().GetRect(), GetRect(), configuration).GetHandle();
    }
    
    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(HOVER);
    }
    
    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }
    
    private void BeginDrag(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(FOLLOW);
    }
    
    private void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.GetGrabber().SetPosition(eventData);
    }

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }
}
