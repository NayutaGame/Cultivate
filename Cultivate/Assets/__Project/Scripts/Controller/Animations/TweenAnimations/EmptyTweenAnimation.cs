
using DG.Tweening;

public class EmptyTweenAnimation : Animation
{
    public EmptyTweenAnimation() : base(false, true)
    {
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill());
    }

    public override bool InvolvesCharacterAnimation()
        => false;
}
