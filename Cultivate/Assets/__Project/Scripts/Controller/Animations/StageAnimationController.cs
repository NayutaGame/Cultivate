
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class StageAnimationController
{
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
    
    private AnimationHandle _mainTrack;
    private List<AnimationHandle> _sideTracks;
    private float _speed = 1;

    private bool _hovering = false;
    private bool _annotating = false;

    public StageAnimationController()
    {
        StageManager.SetHoverToTrue.Join(SetHoveringToTrue);
        StageManager.SetHoverToFalse.Join(SetHoveringToFalse);
        StageManager.SetAnnotatingToTrue.Join(SetAnnotatingToTrue);
        StageManager.SetAnnotatingToFalse.Join(SetAnnotatingToFalse);
    }

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

    public void SetHoveringToTrue(InteractBehaviour ib, PointerEventData d)
    {
        _hovering = true;
        CheckPause();
    }

    public void SetHoveringToFalse(InteractBehaviour ib, PointerEventData d)
    {
        _hovering = false;
        CheckPause();
    }

    public void SetAnnotatingToTrue()
    {
        _annotating = true;
        CheckPause();
    }

    public void SetAnnotatingToFalse()
    {
        _annotating = false;
        CheckPause();
    }

    private void CheckPause()
    {
        if (_hovering || _annotating)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    private void Pause()
    {
        StageManager.Instance.HomeModel.SetSpeed(0);
        StageManager.Instance.AwayModel.SetSpeed(0);
        _mainTrack?.Pause();
    }

    private void Resume()
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
