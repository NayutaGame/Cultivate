
using DG.Tweening;

public class WaitAnimation : Animation
{
    private readonly float _delayInSeconds;
    
    public WaitAnimation(float delayInSeconds) : base(false, true)
    {
        _delayInSeconds = delayInSeconds;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().AppendInterval(_delayInSeconds).SetAutoKill());
    }

    public override bool InvolvesCharacterAnimation()
        => false;
}
