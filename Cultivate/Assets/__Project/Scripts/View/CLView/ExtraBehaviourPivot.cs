
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExtraBehaviourPivot : ExtraBehaviour
{
    public RectTransform IdleTransform;
    public RectTransform HoverTransform;
    public RectTransform FollowTransform;

    private Tween _handle;

    public override void Init(CLView clView)
    {
        base.Init(clView);

        InteractBehaviour ib = CLView.GetInteractBehaviour();
        if (ib == null)
            return;

        ib.PointerEnterNeuron.Join(PointerEnter);
        ib.PointerExitNeuron.Join(PointerExit);
    }

    private void OnDisable()
    {
        _handle?.Kill();
    }

    public RectTransform GetDisplayTransform()
        => CLView.GetDisplayTransform();

    public void SetDisplayTransform(RectTransform pivot)
        => CLView.SetDisplayTransform(pivot);

    private void PointerEnter(InteractBehaviour ib, PointerEventData d)
        => AnimateState(HoverTransform);

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
        => AnimateState(IdleTransform);

    private void SetState(RectTransform end)
    {
        _handle?.Kill();
        SetDisplayTransform(end);
    }

    public void AnimateState(RectTransform start, RectTransform end)
    {
        SetState(start);
        AnimateState(end);
    }

    private void AnimateState(RectTransform end)
    {
        _handle?.Kill();
        FollowAnimation f = new FollowAnimation(GetDisplayTransform(), end);
        _handle = f.GetHandle();
        _handle.SetAutoKill().Restart();
    }

    public void PlayPingAnimation()
    {
        _handle?.Kill();

        _handle = GetDisplayTransform()
            .DOScale(1.5f, 0.075f)
            .SetEase(Ease.OutQuint)
            .SetLoops(2, loopType: LoopType.Yoyo);
        
        _handle.SetAutoKill().Restart();
    }

    public void RefreshPivots()
        => AnimateState(IdleTransform);
}
