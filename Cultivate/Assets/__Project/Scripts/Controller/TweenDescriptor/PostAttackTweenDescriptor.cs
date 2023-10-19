
using DG.Tweening;
using UnityEngine;

public class PostAttackTweenDescriptor : TweenDescriptor
{
    private StageEntity _src;

    public PostAttackTweenDescriptor(bool isAwait, StageEntity src) : base(isAwait)
    {
        _src = src;
    }

    public override Tween GetTween()
    {
        Transform slotTransform = _src.Slot().transform;
        Transform entityTransform = _src.Slot().EntityTransform;
        int orient = -(_src.Index * 2 - 1);

        return DOTween.Sequence()
            .Append(entityTransform.DOMove(slotTransform.position, 0.3f).SetEase(Ease.OutQuad));
    }
}
