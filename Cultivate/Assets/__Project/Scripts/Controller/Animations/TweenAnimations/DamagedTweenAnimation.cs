
using DG.Tweening;

public class DamagedTweenAnimation : Animation
{
    private IStageModel _model;

    public DamagedTweenAnimation(IStageModel model, bool isAwait, bool induced) : base(isAwait, induced)
    {
        _model = model;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill()
            .Append(GetTween()));
    }
    
    public override bool InvolvesCharacterAnimation() => true;

    private Tween GetTween()
    {
        return _model.Transform
            .DOShakeRotation(0.6f, 10 * _model.BaseTransform.forward, 10, 90, true, ShakeRandomnessMode.Harmonic)
            .SetEase(Ease.InQuad);
    }
}
