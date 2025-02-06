
using DG.Tweening;

public class EmptyAnimation : Animation
{
    public EmptyAnimation() : base(false, true)
    {
    }

    public override AnimationHandle GetHandle()
    {
        return new TweenHandle(this, DOTween.Sequence().SetAutoKill());
    }

    public override bool InvolvesCharacterAnimation()
        => false;
}
