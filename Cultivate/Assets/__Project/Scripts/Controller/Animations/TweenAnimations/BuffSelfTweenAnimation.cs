
using DG.Tweening;
using UnityEngine;

public class BuffSelfTweenAnimation : Animation
{
    private IStageModel _model;

    public BuffSelfTweenAnimation(IStageModel model, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _model = model;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill()
            .Append(_model.Transform.DOMove(_model.BaseTransform.position + Vector3.down * 0.2f, 0.2f).SetEase(Ease.OutQuad))
            .Join(_model.Transform.DOScale(new Vector3(1.1f, 0.9f, 1f), 0.2f).SetEase(Ease.OutQuad))
            .Append(_model.Transform.DOMove(_model.BaseTransform.position + Vector3.up * 0.5f, 0.1f).SetEase(Ease.OutQuad))
            .Join(_model.Transform.DOScale(new Vector3(0.9f, 1.1f, 1f), 0.05f).SetEase(Ease.OutQuad))
            .Append(_model.Transform.DOMove(_model.BaseTransform.position, 0.1f).SetEase(Ease.InQuad))
            .Join(_model.Transform.DOScale(1f, 0.05f).SetEase(Ease.OutQuad)));
    }
    
    public override bool InvolvesCharacterAnimation()
        => true;
}
