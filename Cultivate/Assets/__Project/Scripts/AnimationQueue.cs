
using System.Collections.Generic;
using CLLibrary;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class AnimationQueue
{
    private Queue<Tween> _animationQueue;

    public Neuron EnqueueNeuron = new();
    public Neuron DequeueNeuron = new();

    public AnimationQueue()
    {
        _animationQueue = new();
    }

    private void Enqueue(Tween tween)
    {
        _animationQueue.Enqueue(tween);
        EnqueueNeuron.Invoke();
    }

    private Tween Dequeue()
    {
        Tween t = _animationQueue.Dequeue();
        DequeueNeuron.Invoke();
        return t;
    }

    public void CompleteAnimationQueue()
    {
        while (_animationQueue.Count > 0)
        {
            Tween t = _animationQueue.Peek();
            t.Complete();
            Dequeue();
        }
    }

    public async UniTask WaitForQueueToComplete()
    {
        while (_animationQueue.Count > 0)
            await _animationQueue.Peek().AsyncWaitForCompletion();
    }

    public async UniTask TryProcessAnimationQueue()
    {
        bool queueIsEmpty = _animationQueue.Count == 0;
        if (queueIsEmpty)
            return;

        bool isPlaying = _animationQueue.Peek().IsPlaying();
        if (isPlaying)
            return;
        
        while (_animationQueue.Count > 0)
        {
            Tween t = _animationQueue.Peek();
            t.SetAutoKill().Restart();
            await t.AsyncWaitForCompletion();
            Dequeue();
        }
    }

    public void QueueAnimation(Tween tween)
    {
        Enqueue(tween);
        TryProcessAnimationQueue();
    }

    public int Count() => _animationQueue.Count;
}
