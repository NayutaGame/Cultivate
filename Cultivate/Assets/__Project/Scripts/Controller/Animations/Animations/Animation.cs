
public abstract class Animation
{
    public readonly bool IsAwait;
    public readonly bool Induced;

    public Animation(bool isAwait, bool induced)
    {
        IsAwait = isAwait;
        Induced = induced;
    }

    public abstract AnimationHandle GetHandle();
    public abstract bool InvolvesCharacterAnimation();
}
