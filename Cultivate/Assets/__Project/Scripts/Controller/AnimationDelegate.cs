using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public interface AnimationDelegate
{
    Task PlayTween(TweenDescriptor descriptor);
    Task PlayTween(Tween tween);
}
