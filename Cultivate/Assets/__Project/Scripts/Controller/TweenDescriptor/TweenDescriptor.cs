
using DG.Tweening;

public abstract class TweenDescriptor
{
    protected bool _isAwait;
    public bool IsAwait() => _isAwait;

    public TweenDescriptor(bool isAwait)
    {
        _isAwait = isAwait;
    }

    public abstract Tween GetTween();
}
