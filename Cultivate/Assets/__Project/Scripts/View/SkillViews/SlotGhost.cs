
using DG.Tweening;
using UnityEngine;

public class SlotGhost : SlotView
{
    [SerializeField] public CanvasGroup CanvasGroup;

    private Tween _animationHandle;
    public bool IsAnimating
        => _animationHandle != null && _animationHandle.active;

    public void SetPivot(RectTransform pivot)
    {
        _animationHandle?.Kill();
        FollowAnimation f = new FollowAnimation(RectTransform, pivot);
        _animationHandle = f.GetHandle();
        _animationHandle.SetAutoKill().Restart();
    }

    public void SetPivotWithoutAnimation(RectTransform pivot)
    {
        _animationHandle?.Kill();
        RectTransform.position = pivot.position;
        RectTransform.localScale = pivot.localScale;
    }
}
