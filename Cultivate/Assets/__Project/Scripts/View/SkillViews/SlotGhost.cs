
using DG.Tweening;
using UnityEngine;

public class SlotGhost : SlotView
{
    [SerializeField] public CanvasGroup CanvasGroup;

    private Tween _animationHandle;
    public bool IsAnimating
        => _animationHandle != null && _animationHandle.active;

    public void GoToPivot(RectTransform pivot)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(RectTransform, pivot);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }
}
