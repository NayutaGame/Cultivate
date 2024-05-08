
using DG.Tweening;
using UnityEngine;

public class PostAttackAnimation : Animation
{
    private StageEntity _src;

    public PostAttackAnimation(bool isAwait, StageEntity src) : base(isAwait)
    {
        _src = src;
    }

    public override AnimationHandle GetHandle()
    {
        Transform slotTransform = _src.Slot().transform;
        Transform entityTransform = _src.Slot().EntityTransform;
        int orient = -(_src.Index * 2 - 1);

        return new TweenHandle(this, DOTween.Sequence()
            .Append(entityTransform.DOMove(slotTransform.position, 0.3f).SetEase(Ease.OutQuad)));
    }
}
