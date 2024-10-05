
using System.Threading.Tasks;
using DG.Tweening;

public class TweenHandle : AnimationHandle
{
    private Tween _handle;

    public TweenHandle(Animation animation, Tween handle) : base(animation)
    {
        _handle = handle;
    }
    
    public override void Play()
    {
        _handle.SetAutoKill().Restart();
    }

    public override async Task NextKey(float speed)
    {
        if (!_animation.IsAwait)
            return;
        await _handle.AsyncWaitForCompletion();
    }

    public override void Pause()
        => _handle.Pause();

    public override void Resume(float speed)
        => _handle.Play();

    public override void SetSpeed(float speed)
        => _handle.timeScale = speed;

    public override void Skip()
    {
        // _tween.Complete();
    }
}
