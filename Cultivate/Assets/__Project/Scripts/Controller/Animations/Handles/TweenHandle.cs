
using System.Threading.Tasks;
using DG.Tweening;

public class TweenHandle : AnimationHandle
{
    private Tween _handle;

    public TweenHandle(Animation animation, Tween handle) : base(animation)
    {
        _handle = handle;
    }
    
    public override async Task Play()
    {
        _handle.timeScale = _speed;
        _handle.SetAutoKill().Restart();
        if (_animation.IsAwait())
            await _handle.AsyncWaitForCompletion();
    }

    public override void Pause()
        => _handle.Pause();

    public override void Resume()
        => _handle.Play();

    public override void SetSpeed(float speed)
    {
        base.SetSpeed(speed);
        _handle.timeScale = _speed;
    }

    public override void Skip()
    {
        // _tween.Complete();
    }
}
