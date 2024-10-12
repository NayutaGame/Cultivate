
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Spine.Unity;

public class SpineHandle : AnimationHandle
{
    private SkeletonAnimation _skeleton;
    private Tween _handle;

    private Action _playFunc;

    private int _index;
    private float[] _intervals;
    
    public SpineHandle(Animation animation, float[] intervals, SkeletonAnimation skeleton, Action playFunc) : base(animation)
    {
        _index = 0;
        _intervals = intervals ?? Array.Empty<float>();
        _skeleton = skeleton;
        _playFunc = playFunc;
    }
    
    public override void Play()
    {
        _playFunc();
    }

    public override async UniTask NextKey(float speed)
    {
        if (!_animation.IsAwait)
            return;
        
        if (_index < _intervals.Length)
        {
            _handle = DOTween.Sequence()
                .AppendInterval(_intervals[_index]).AppendCallback(() => { });
            _handle.timeScale = speed;
            _handle.SetAutoKill().Restart();
            _index++;

            await _handle.AsyncWaitForCompletion();
        }
    }

    public override void Pause()
    {
        _skeleton.timeScale = 0;
        _handle?.Pause();
    }

    public override void Resume(float speed)
    {
        _skeleton.timeScale = speed;
        _handle?.Play();
    }

    public override void SetSpeed(float speed)
    {
        _skeleton.timeScale = speed;
        if (_handle != null)
            _handle.timeScale = speed;
    }

    public override void Skip()
    {
        // _tween.Complete();
    }
}
