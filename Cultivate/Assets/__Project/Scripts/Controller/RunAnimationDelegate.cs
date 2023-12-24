
using System.Collections.Generic;
using DG.Tweening;

public class RunAnimationDelegate
{
    private Queue<RunTweenDescriptor> _queue;
    private Tween _handle;
    public bool IsAnimating => _handle != null && _handle.active;

    public void QueueTween(RunTweenDescriptor descriptor)
    {
        _queue.Enqueue(descriptor);
        TryNextTween();
    }

    private void SkipCurrTween()
    {
        if (IsAnimating)
            _handle.Complete();
    }

    private void TryNextTween()
    {
        if (IsAnimating)
            return;

        _handle?.Kill();// complete
        if (!_queue.TryDequeue(out RunTweenDescriptor next))
            return;

        _handle = next.GetTween().OnComplete(TryNextTween).SetAutoKill();
        _handle.Restart();
    }
}
