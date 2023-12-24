
using DG.Tweening;
using UnityEngine;

public class PreAttackTweenDescriptor : StageTweenDescriptor
{
    private StageEntity _src;

    public PreAttackTweenDescriptor(bool isAwait, StageEntity src) : base(isAwait)
    {
        _src = src;
    }

    public override Tween GetTween()
    {
        Transform slotTransform = _src.Slot().transform;
        Transform entityTransform = _src.Slot().EntityTransform;
        int orient = -(_src.Index * 2 - 1);
        return entityTransform.DOMove(slotTransform.position + Vector3.right * orient * -0.2f, 0.05f).SetEase(Ease.OutQuad);
    }
}
