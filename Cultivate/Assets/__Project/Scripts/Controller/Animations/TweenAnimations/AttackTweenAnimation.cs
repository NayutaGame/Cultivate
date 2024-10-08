
using DG.Tweening;
using UnityEngine;

public class AttackTweenAnimation : Animation
{
    private Transform _baseTransform;
    private Transform _transform;

    public AttackTweenAnimation(Transform baseTransform, Transform transform, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _baseTransform = baseTransform;
        _transform = transform;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill()
            .Append(GetAnticipationTween())
            .Append(GetAttackTween()));
    }
    
    public override bool InvolvesCharacterAnimation() => true;

    private Tween GetAnticipationTween()
    {
        return DOTween.Sequence()
            .Append(_transform.DOMove(_baseTransform.position + _baseTransform.right * -0.2f, 0.2f).SetEase(Ease.OutQuad));
    }

    private Tween GetAttackTween()
    {
        return DOTween.Sequence()
            .Append(_transform.DOMove(_baseTransform.position + _baseTransform.right * 0.5f, 0.1f).SetEase(Ease.OutQuad))
            .Append(_transform.DOMove(_baseTransform.position, 0.1f).SetEase(Ease.InQuad));
    }
}
