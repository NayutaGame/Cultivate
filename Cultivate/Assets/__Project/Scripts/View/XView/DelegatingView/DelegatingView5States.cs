
using DG.Tweening;
using FMOD;
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
    
    private void Drag(InteractBehaviour ib, PointerEventData eventData)
    {
        CanvasManager.Instance.GetGrabber().SetPosition(eventData);
    }
}
