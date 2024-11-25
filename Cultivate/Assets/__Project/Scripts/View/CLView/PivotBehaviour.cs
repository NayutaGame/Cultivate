
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PivotBehaviour : XBehaviour
{
    public RectTransform IdleTransform;
    public RectTransform HoverTransform;
    public RectTransform FollowTransform;

    private Tween _handle;

    private Animator _animator;
    public Animator Animator => _animator;

    public RectTransform GetDisplayTransform()
        => View.GetViewTransform();

    public override void Init(XView view)
    {
        base.Init(view);

        _animator = InitAnimator();

        InteractBehaviour ib = View.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
        ib.DraggingExitNeuron.Join(DraggingExit);
    }

    private void OnDisable()
    {
        _handle?.Kill();
    }
    
    protected Animator InitAnimator()
    {
        // 0 for hide, 1 for idle, 2 for hover, 3 for follow, 4 for ping
        Animator animator = new(5, View.name);
        animator[-1, 0] = HideTween;
        animator[0, -1] = SetInteractable;
        animator[-1, 1] = IdleTween;
        animator[-1, 2] = HoverTween;
        animator[-1, 3] = FollowTween;
        animator[-1, 4] = PingTween;
        return animator;
    }

    public void PlayAppearAnimation()
    {
        _animator.SetState(0);
        _animator.SetStateAsync(1);
    }
    
    public void PlayDisappearAnimation()
    {
        _animator.SetStateAsync(0);
    }

    public void Disappear()
    {
        _animator.SetState(0);
    }

    public void RefreshPivots()
        => _animator.SetStateAsync(1);

    private void DraggingExit(InteractBehaviour from, InteractBehaviour to, PointerEventData d)
    {
        // if (_animator.State != 0)
        //     _animator.SetStateAsync(1);
    }

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
    {
        // _animator.SetStateAsync(2);
    }

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
    {
        // if (_animator.State != 0)
        //     _animator.SetStateAsync(1);
    }

    public void PlayPingAnimation()
        => _animator.SetStateAsync(4);

    public void RectTransformToIdle(RectTransform rectTransform)
    {
        FollowTransform.position = rectTransform.position;
        FollowTransform.localScale = rectTransform.localScale;
        _animator.SetState(3);
        _animator.SetStateAsync(1);
    }

    public void PositionToIdle(Vector3 position)
    {
        FollowTransform.position = position;
        _animator.SetState(3);
        _animator.SetStateAsync(1);
    }

    private Tween HideTween()
        => DOTween.Sequence()
            .AppendCallback(() => View.GetInteractBehaviour().SetInteractable(false))
            .Append(GetDisplayTransform().DOScale(0, 0.15f).SetEase(Ease.OutQuad));

    private Tween IdleTween()
        => DOTween.Sequence()
            .Append(new FollowAnimation(GetDisplayTransform(), IdleTransform).GetHandle());
            // .AppendCallback(() => CLView.GetInteractBehaviour().SetInteractable(true))

    private Tween SetInteractable()
        => DOTween.Sequence()
            .AppendCallback(() => View.GetInteractBehaviour().SetInteractable(true));

    private Tween HoverTween()
        => new FollowAnimation(GetDisplayTransform(), HoverTransform).GetHandle();

    private Tween FollowTween()
        => new FollowAnimation(GetDisplayTransform(), FollowTransform).GetHandle();

    private Tween PingTween()
        => GetDisplayTransform().DOScale(1.5f, 0.075f).SetEase(Ease.OutQuad).SetLoops(2, loopType: LoopType.Yoyo);
}
