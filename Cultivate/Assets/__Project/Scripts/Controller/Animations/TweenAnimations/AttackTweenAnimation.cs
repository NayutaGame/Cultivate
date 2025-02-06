
using DG.Tweening;

public class AttackTweenAnimation : Animation
{
    private IStageModel _model;

    public AttackTweenAnimation(IStageModel model, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _model = model;
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
            .Append(_model.Transform.DOMove(_model.BaseTransform.position + _model.BaseTransform.right * -0.2f, 0.2f).SetEase(Ease.OutQuad));
    }

    private Tween GetAttackTween()
    {
        return DOTween.Sequence()
            .Append(_model.Transform.DOMove(_model.BaseTransform.position + _model.BaseTransform.right * 0.5f, 0.1f).SetEase(Ease.OutQuad))
            .Append(_model.Transform.DOMove(_model.BaseTransform.position, 0.1f).SetEase(Ease.InQuad));
    }
}
