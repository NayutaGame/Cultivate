
using DG.Tweening;

public abstract class StageTweenDescriptor
{
    protected bool _isAwait;
    public bool IsAwait() => _isAwait;

    public StageTweenDescriptor(bool isAwait)
    {
        _isAwait = isAwait;
    }

    public abstract Tween GetTween();
}
