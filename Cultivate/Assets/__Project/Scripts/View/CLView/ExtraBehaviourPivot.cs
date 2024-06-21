
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
        => SetTargetAnimated(HoverTransform);

    private void PointerExit(InteractBehaviour ib, PointerEventData d)
        => SetTargetAnimated(IdleTransform);

    private void SetTarget(RectTransform end)
    {
        _handle?.Kill();
        SetDisplayTransform(end);
    }

    private void SetTargetAnimated(RectTransform end)
    {
        _handle?.Kill();
        FollowAnimation f = new FollowAnimation(GetDisplayTransform(), end);
        _handle = f.GetHandle();
        _handle.SetAutoKill().Restart();
    }

    public void SetPathAnimated(RectTransform start, RectTransform end)
    {
        SetTarget(start);
        SetTargetAnimated(end);
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
        => SetTargetAnimated(IdleTransform);
}
