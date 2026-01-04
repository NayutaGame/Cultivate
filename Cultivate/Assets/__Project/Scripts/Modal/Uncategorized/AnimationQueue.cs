
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class AnimationQueue
{
    public enum AnimationQueueStatus
    {
        Stopped,
        Playing,
    }
    
    private Queue<Tween> _animationQueue;
    private AnimationQueueStatus _status;
    private Tween _handle;

    public bool IsPlaying => _status == AnimationQueueStatus.Playing;

    public AnimationQueue()
    {
        _animationQueue = new();
        _status = AnimationQueueStatus.Stopped;
        _handle = null;
    }
    
    private async UniTask ProcessAnimationQueue()
    {
        if (_status == AnimationQueueStatus.Playing)
            return;

        if (_animationQueue.Count == 0)
            return;

        _status = AnimationQueueStatus.Playing;
        
        while (_animationQueue.Count > 0)
        {
            _handle = _animationQueue.Dequeue();
            _handle.SetAutoKill().Restart();
            await _handle.AsyncWaitForCompletion();
        }

        _handle = null;
        _status = AnimationQueueStatus.Stopped;
    }

    public async UniTask WaitForQueueToComplete()
    {
        if (_status == AnimationQueueStatus.Stopped)
            return;
        
        while (_animationQueue.Count > 0)
            await _animationQueue.Peek().AsyncWaitForCompletion();
        
        if (_status == AnimationQueueStatus.Playing && _handle != null && _handle.IsActive())
            await _handle.AsyncWaitForCompletion();
    }

    public void QueueAnimation(Tween tween)
    {
        _animationQueue.Enqueue(tween);
        ProcessAnimationQueue().Forget(UnityEngine.Debug.LogException);
    }

    public void CompleteCurrAnimation()
    {
        if (_status == AnimationQueueStatus.Stopped)
            return;

        bool hasNextAnimation = _animationQueue.Count > 0;
        if (hasNextAnimation)
        {
            _status = AnimationQueueStatus.Playing;
            _handle.Complete(true);
            _handle = _animationQueue.Dequeue();
            _handle.Restart();
        }
        else
        {
            _status = AnimationQueueStatus.Stopped;
            _handle.Complete(true);
            _handle = null;
        }
    }

    public void CompleteAllAnimations()
    {
        if (_status == AnimationQueueStatus.Stopped)
            return;

        _status = AnimationQueueStatus.Stopped;

        _handle.Complete(true);
        
        while(_animationQueue.Count > 0)
        {
            _animationQueue.Dequeue().Complete(true);
        }

        _handle = null;
    }
    
    public void QueueInterval(float milliseconds)
    {
        Tween tween = DOTween.Sequence().AppendInterval(milliseconds / 1000f);
        QueueAnimation(tween);
    }
    
    public void QueueSignal(Signal signal)
    {
        Tween tween = DOTween.Sequence().AppendCallback(() => RunManager.Instance.Environment.Map.ReceiveSignalProcedure(signal));
        QueueAnimation(tween);
    }

    public int Count() => _animationQueue.Count;
}
