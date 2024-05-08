
using DG.Tweening;
using UnityEngine;

public class EvadeAnimation : Animation
{
    private StageEntity _tgt;

    public EvadeAnimation(bool isAwait, StageEntity tgt) : base(isAwait)
    {
        _tgt = tgt;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill()
            // .AppendCallback(SpawnHitVFX)
            // .AppendCallback(SpawnAttackedText)
            .Append(GetDodgeTween())
            .AppendInterval(0.2f)
            .Append(GetBackTween()));
    }

    private Tween GetDodgeTween()
    {
        Transform slotTransform = _tgt.Slot().transform;
        Transform entityTransform = _tgt.Slot().EntityTransform;
        int orient = -(_tgt.Opponent().Index * 2 - 1);
        return entityTransform.DOMove(slotTransform.position + Vector3.right * orient * 0.6f, 0.05f)
            .SetEase(Ease.OutQuad);
    }

    private Tween GetBackTween()
    {
        Transform slotTransform = _tgt.Slot().transform;
        Transform entityTransform = _tgt.Slot().EntityTransform;
        int orient = -(_tgt.Opponent().Index * 2 - 1);
        return entityTransform.DOMove(slotTransform.position + Vector3.right * -orient * 0.6f, 0.05f)
            .SetEase(Ease.InQuad);
    }
}
