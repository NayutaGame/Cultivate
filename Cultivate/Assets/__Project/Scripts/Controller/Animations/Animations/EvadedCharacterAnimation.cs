
using DG.Tweening;
using UnityEngine;

public class EvadedCharacterAnimation : Animation
{
    private StageEntity _tgt;

    public EvadedCharacterAnimation(bool isAwait, EvadedDetails d) : base(isAwait, d.Induced)
    {
        _tgt = d.Tgt;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill()
            .Append(GetDodgeTween())
            .AppendInterval(0.2f)
            .Append(GetBackTween()));
    }
    
    public override bool InvolvesCharacterAnimation() => true;

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
