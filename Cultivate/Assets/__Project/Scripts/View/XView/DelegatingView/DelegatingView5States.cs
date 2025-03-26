
using DG.Tweening;
using FMOD;
using UnityEngine;
using UnityEngine.EventSystems;

public class DelegatingView5States : DelegatingView
{
    public static readonly int DEFAULT = -1;
    public static readonly int HIDE = 0;
    public static readonly int IDLE = 1;
    public static readonly int HOVER = 2;
    public static readonly int FOLLOW = 3;
    public static readonly int FREE = 4;
    
    protected override Animator InitAnimator()
    {
        Animator animator = new(5, name);
        animator[DEFAULT, HIDE] = EnterHide;
        animator[DEFAULT, IDLE] = EnterIdle;
        animator[DEFAULT, HOVER] = EnterHover;
        animator[DEFAULT, FOLLOW] = EnterFollow;
        animator[DEFAULT, FREE] = EnterFree;
        animator[FREE, DEFAULT] = ExitFree;
        return animator;
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

    private Tween EnterFollow()
        => DOTween.Sequence()
            .AppendCallback(GrabberSetDrag)
            .Append(new FollowAnimation(GetDelegatedView().GetRect(), CanvasManager.Instance.GetGrabber().GetRect()).GetHandle());

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
    
    public void EndDrag(InteractBehaviour ib, PointerEventData d)
    {
        GetAnimator().SetStateAsync(IDLE);
    }
    
    private void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.GetGrabber().SetPosition(eventData);
    }

    // private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    // {
    //     GetAnimator().SetStateAsync(1);
    // }

    public void GrabberSetHover()
    {
        CanvasManager.Instance.GetGrabber().SetHover(this);
    }

    public void GrabberSetDrag()
    {
        CanvasManager.Instance.GetGrabber().SetDrag(this);
    }

    public void GrabberRelease()
    {
        CanvasManager.Instance.GetGrabber().Release(this);
    }
}
