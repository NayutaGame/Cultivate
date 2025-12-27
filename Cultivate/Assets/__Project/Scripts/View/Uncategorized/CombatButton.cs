
using UnityEngine;
using UnityEngine.UI;

public class CombatButton : XView
{
    [SerializeField] public Image IconPlaceHolder;
    [SerializeField] public Sprite[] Icons;
    [SerializeField] public CLButton Button;

    // private static Tween IdleTween(RectTransform target)
    //     => TweenAnimation.Jump(target);
    //
    // private static Tween AttractiveTween(RectTransform target)
    //     => TweenAnimation.Beats(target);
    //
    // private static Tween HoverTween(RectTransform target)
    //     => target.DOScale(1.2f * Vector3.one, 0.2f).SetEase(Ease.OutQuad);
    //
    // private static Tween AttractiveHoverTween(RectTransform target)
    //     => TweenAnimation.HarshBeats(target);
}
