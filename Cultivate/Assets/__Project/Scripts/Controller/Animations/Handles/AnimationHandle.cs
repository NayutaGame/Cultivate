
using System.Threading.Tasks;

public abstract class AnimationHandle
{
    protected Animation _animation;
    protected float _speed;

    public AnimationHandle(Animation animation)
    {
        _animation = animation;
        _speed = 1;
    }

    public virtual async Task Play() { }

    public abstract void Pause();

    public abstract void Resume();

    public virtual void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public virtual void Skip()
    {
    }
}
