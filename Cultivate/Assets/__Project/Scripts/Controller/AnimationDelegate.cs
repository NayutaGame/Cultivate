
using System.Threading.Tasks;
using DG.Tweening;

public interface AnimationDelegate
{
    Task PlayTween(TweenDescriptor descriptor);
    Task PlayTween(bool isAwait, Tween tween);
}
