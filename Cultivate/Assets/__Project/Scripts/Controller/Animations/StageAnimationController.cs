
using System.Collections.Generic;
using System.Threading.Tasks;

public class StageAnimationController
{
    private AnimationHandle _handle;
    
    private AnimationHandle _shift;
    private AnimationHandle _home;
    private AnimationHandle _away;
    // health bar
    // buff icon
    private List<AnimationHandle> _vfxList;
    
    private float _speed = 1;

    public async Task Play(Animation animation)
    {
        _handle = animation.GetHandle();
        _handle.SetSpeed(_speed);
        await _handle.Play();

        CanvasManager.Instance.StageCanvas.Refresh();

        // opening
        // bullet time at killing moment
        // speed control and skip
        // camera shake when attack with large value
    }

    public void Pause() => _handle.Pause();
    public void Resume() => _handle.Resume();
    public void SetSpeed(float speed)
    {
        _speed = speed;
        _handle.SetSpeed(_speed);
    }

    public void Skip() => _handle.Skip();
}
