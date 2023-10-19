
using System.Threading.Tasks;
using DG.Tweening;

public class StageAnimationDelegate : AnimationDelegate
{
    public TimelineView TimelineView;
    private Tween _tween;
    private float _speed;

    public StageAnimationDelegate()
    {
        _speed = 1;
    }

    public async Task PlayTween(TweenDescriptor descriptor)
    {
        if (descriptor is ShiftTweenDescriptor shift)
        {
            if (TimelineView != null)
            {
                Tween shiftTween = TimelineView.ShiftAnimation();
                await PlayTween(true, shiftTween);
            }
        }
        else
        {
            await PlayTween(descriptor.IsAwait(), descriptor.GetTween());
        }

        StageCanvas.Instance.Refresh();

        // opening
        // bullet time at killing moment
        // speed control and skip
        // camera shake when attack with large value
    }

    public async Task PlayTween(bool isAwait, Tween tween)
    {
        _tween = tween;
        _tween.timeScale = _speed;
        _tween.SetAutoKill().Restart();
        if (isAwait)
            await _tween.AsyncWaitForCompletion();
    }

    public void PauseTween()
    {
        _tween.Pause();
    }

    public void ResumeTween()
    {
        _tween.Play();
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        _tween.timeScale = _speed;
    }

    public void Skip()
    {
        // _tween.Complete();
        // Anim.Skip();
    }
}
