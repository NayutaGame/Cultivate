
using DG.Tweening;
using UnityEngine;

public class BuffSelfTweenAnimation : Animation
{
    private Transform _baseTransform;
    private Transform _transform;

    public BuffSelfTweenAnimation(Transform baseTransform, Transform transform, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _baseTransform = baseTransform;
        _transform = transform;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill()
            .Append(_transform.DOMove(_baseTransform.position + Vector3.down * 0.2f, 0.2f).SetEase(Ease.OutQuad))
            .Join(_transform.DOScale(new Vector3(1.3f, 0.8f, 1f), 0.2f).SetEase(Ease.OutQuad))
            .Append(_transform.DOMove(_baseTransform.position + Vector3.up * 0.5f, 0.1f).SetEase(Ease.OutQuad))
            .Join(_transform.DOScale(new Vector3(0.8f, 1.3f, 1f), 0.05f).SetEase(Ease.OutQuad))
            .Append(_transform.DOMove(_baseTransform.position, 0.1f).SetEase(Ease.InQuad))
            .Join(_transform.DOScale(1f, 0.05f).SetEase(Ease.OutQuad)));
    }
    
    public override bool InvolvesCharacterAnimation()
        => true;
}
