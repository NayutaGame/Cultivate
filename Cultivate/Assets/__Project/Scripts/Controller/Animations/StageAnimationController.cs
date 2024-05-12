
using System.Collections.Generic;
using System.Threading.Tasks;

public class StageAnimationController
{
    private AnimationHandle _mainTrack;
    private List<AnimationHandle> _sideTracks;
    
    // shift
    // home
    // away
    // health bar
    // buff icon
    // vfxs
    // floating texts
    
    // opening
    // bullet time at killing moment
    // camera shake when attack with large value
    
    private float _speed = 1;

    public async Task Play(Animation animation)
    {
        AnimationHandle track = animation.GetHandle();
        track.SetSpeed(_speed);
        await track.Play();
        
        if (animation.IsAwait())
        {
            _mainTrack = track;
        }
        else
        {
            // _sideTracks.Add(track);
        }

        CanvasManager.Instance.StageCanvas.Refresh();
    }

    public async Task NextKey()
    {
        await Task.Delay(1000);
    }

    public void Pause() => _mainTrack?.Pause();
    public void Resume() => _mainTrack?.Resume();
    public void SetSpeed(float speed)
    {
        _speed = speed;
        _mainTrack?.SetSpeed(_speed);
    }

    public void Skip() => _mainTrack?.Skip();
}
