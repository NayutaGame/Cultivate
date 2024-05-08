
using DG.Tweening;
using UnityEngine;

public class PreAttackAnimation : Animation
{
    private StageEntity _src;

    public PreAttackAnimation(bool isAwait, StageEntity src) : base(isAwait)
    {
        _src = src;
    }

    public override AnimationHandle GetHandle()
    {
        Transform slotTransform = _src.Slot().transform;
        Transform entityTransform = _src.Slot().EntityTransform;
        int orient = -(_src.Index * 2 - 1);
        return new TweenHandle(this, entityTransform.DOMove(slotTransform.position + Vector3.right * orient * -0.2f, 0.05f).SetEase(Ease.OutQuad));
    }
}
