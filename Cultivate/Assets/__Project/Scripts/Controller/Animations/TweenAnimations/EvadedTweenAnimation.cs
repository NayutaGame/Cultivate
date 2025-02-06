
using DG.Tweening;

public class EvadedTweenAnimation : Animation
{
    private IStageModel _model;

    public EvadedTweenAnimation(IStageModel model, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _model = model;
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
        return _model.Transform.DOMove(_model.BaseTransform.position + _model.BaseTransform.right * -0.6f, 0.05f)
            .SetEase(Ease.OutQuad);
    }

    private Tween GetBackTween()
    {
        return _model.Transform.DOMove(_model.BaseTransform.position, 0.05f)
            .SetEase(Ease.InQuad);
    }
}
