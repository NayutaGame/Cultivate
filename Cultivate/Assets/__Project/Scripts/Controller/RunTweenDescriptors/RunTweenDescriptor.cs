
using DG.Tweening;

public abstract class RunTweenDescriptor
{
    protected bool _isAwait;
    public bool IsAwait() => _isAwait;

    public RunTweenDescriptor(bool isAwait)
    {
        _isAwait = isAwait;
    }

    public abstract Tween GetTween();
}
