
using DG.Tweening;
using UnityEngine;

public class EvadedTweenAnimation : Animation
{
    private Transform _baseTransform;
    private Transform _transform;

    public EvadedTweenAnimation(Transform baseTransform, Transform transform, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _baseTransform = baseTransform;
        _transform = transform;
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
        return _transform.DOMove(_baseTransform.position + _baseTransform.right * -0.6f, 0.05f)
            .SetEase(Ease.OutQuad);
    }

    private Tween GetBackTween()
    {
        return _transform.DOMove(_baseTransform.position, 0.05f)
            .SetEase(Ease.InQuad);
    }
}
