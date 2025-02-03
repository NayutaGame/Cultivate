
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

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

    public async UniTask Play(Animation animation)
    {
        AnimationHandle track = animation.GetHandle();
        track.SetSpeed(_speed);
        track.Play();
        
        if (animation.IsAwait)
        {
            _mainTrack = track;
        }
        else
        {
            // _sideTracks.Add(track);
        }

        await track.NextKey(_speed);

        // CanvasManager.Instance.StageCanvas.Refresh();
    }

    public async UniTask NextKey()
    {
        await _mainTrack.NextKey(_speed);
    }

    public void Pause()
    {
        StageManager.Instance.HomeModel.SetSpeed(0);
        StageManager.Instance.AwayModel.SetSpeed(0);
        _mainTrack?.Pause();
    }

    public void Resume()
    {
        StageManager.Instance.HomeModel.SetSpeed(_speed);
        StageManager.Instance.AwayModel.SetSpeed(_speed);
        _mainTrack?.Resume(_speed);
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
        StageManager.Instance.HomeModel.SetSpeed(_speed);
        StageManager.Instance.AwayModel.SetSpeed(_speed);
        _mainTrack?.SetSpeed(_speed);
        // _sideTracks
    }

    public void Skip() => _mainTrack?.Skip();
}
