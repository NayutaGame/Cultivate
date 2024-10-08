
using DG.Tweening;
using UnityEngine;

public class DamagedTweenAnimation : Animation
{
    private Transform _baseTransform;
    private Transform _transform;

    public DamagedTweenAnimation(Transform baseTransform, Transform transform, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _baseTransform = baseTransform;
        _transform = transform;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill()
            .Append(GetTween()));
    }
    
    public override bool InvolvesCharacterAnimation() => true;

    private Tween GetTween()
    {
        return _transform
            .DOShakeRotation(0.6f, 10 * _baseTransform.forward, 10, 90, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InQuad);
    }
}
