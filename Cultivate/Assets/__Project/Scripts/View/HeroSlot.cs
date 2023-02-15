using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HeroSlot : EntitySlot
{
    public override Tween GetAttackTween()
    {
        return DOTween.Sequence()
            .Append(EntityView.DOMove(transform.position + Vector3.right, 0.3f).SetEase(Ease.InBack))
            .Append(EntityView.DOMove(transform.position, 0.3f).SetEase(Ease.OutQuad))
            .SetAutoKill();
    }

    public override Tween GetAttackedTween()
    {
        return EntityView.DOShakeRotation(0.6f, 10 * Vector3.back, 10, 90, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InQuad).SetDelay(0.3f).SetAutoKill();
    }
}
