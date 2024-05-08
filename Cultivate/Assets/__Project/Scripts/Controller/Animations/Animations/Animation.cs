
public abstract class Animation
{
    protected bool _isAwait;
    public bool IsAwait() => _isAwait;

    public Animation(bool isAwait)
    {
        _isAwait = isAwait;
    }

    public abstract AnimationHandle GetHandle();
}
