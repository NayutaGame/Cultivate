
using System.Collections.Generic;
using DG.Tweening;

public class RunAnimationController
{
    // private Queue<Animation> _queue;
    // private Tween _handle;
    // public bool IsAnimating => _handle != null && _handle.active;
    //
    // public void Queue(Animation animation)
    // {
    //     _queue.Enqueue(animation);
    //     TryNext();
    // }
    //
    // private void SkipCurrent()
    // {
    //     if (IsAnimating)
    //         _handle.Complete();
    // }
    //
    // private void TryNext()
    // {
    //     if (IsAnimating)
    //         return;
    //
    //     _handle?.Kill();// complete
    //     if (!_queue.TryDequeue(out Animation next))
    //         return;
    //
    //     _handle = next.GetHandle().OnComplete(TryNext).SetAutoKill();
    //     _handle.Restart();
    // }
}
