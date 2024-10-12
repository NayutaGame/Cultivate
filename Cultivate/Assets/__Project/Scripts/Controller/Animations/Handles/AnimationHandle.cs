
using Cysharp.Threading.Tasks;

public abstract class AnimationHandle
{
    protected Animation _animation;

    public AnimationHandle(Animation animation)
    {
        _animation = animation;
    }

    public abstract void Play();
    
    public virtual async UniTask NextKey(float speed) { }

    public abstract void Pause();

    public abstract void Resume(float speed);

    public abstract void SetSpeed(float speed);

    public virtual void Skip()
    {
    }
}
