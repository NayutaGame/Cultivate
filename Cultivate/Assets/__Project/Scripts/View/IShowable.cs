using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface IShowable
{
    bool IsShowing();
    void SetShowing(bool showing);
    Tween GetShowTween();
    Tween GetHideTween();
}
