
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DelegatingView3States : DelegatingView
{
    public static readonly int ANY = -1;
    public static readonly int HIDE = 0;
    public static readonly int IDLE = 1;
    public static readonly int HOVER = 2;
    
    protected override Animator InitAnimator()
    {
        Animator animator = new(3);
        animator[ANY, HIDE] = EnterHide;
        animator[ANY, IDLE] = EnterIdle;
        animator[ANY, HOVER] = EnterHover;
        return animator;
    }

    protected override void InitInteractBehaviour(InteractBehaviour ib)
    {
        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
        ib.DraggingExitNeuron.Join(DraggingExit);
    }

    [SerializeField] private Configuration HideConfiguration = new(localScale: Vector3.zero);
    [SerializeField] private Configuration IdleConfiguration = new(localScale: 0.5f * Vector3.one);
    [SerializeField] private Configuration HoverConfiguration = new(localScale: 0.75f * Vector3.one);

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
        GetAnimator().SetStateAsync(HOVER);
    }
    
    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }

    private void GrabberSetHover()
    {
        CanvasManager.Instance.GetGrabber().SetHover(this);
    }

    private void GrabberRelease()
    {
        CanvasManager.Instance.GetGrabber().Release(this);
    }
    private void OnDisable()
    {
        GrabberRelease();
    }
}
