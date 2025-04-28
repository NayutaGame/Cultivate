
using DG.Tweening;

public class WaitAnimation : Animation
{
    private readonly float _delayInSeconds;
    
    public WaitAnimation(float delayInSeconds) : base(true, true)
    {
        _delayInSeconds = delayInSeconds;
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence()
            .Append(DOTween.To(Getter, Setter, 1, _delayInSeconds).From(0))
            .SetAutoKill());
    }

    private float _t;

    private float Getter() => _t;
    private void Setter(float value) => _t = value;

    public override bool InvolvesCharacterAnimation()
        => false;
}
