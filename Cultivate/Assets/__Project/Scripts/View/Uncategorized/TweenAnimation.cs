
using DG.Tweening;
using UnityEngine;

public class TweenAnimation
{
    public static Tween Show(RectTransform target, Vector2 idlePosition)
    {
        return target.DOAnchorPos(idlePosition, 0.15f).From(idlePosition + Vector2.right * 100).SetEase(Ease.OutQuad);
    }
    
    public static Tween Show(RectTransform target, Vector2 idlePosition, CanvasGroup canvasGroup)
    {
        return DOTween.Sequence()
            .Append(target.DOAnchorPos(idlePosition, 0.15f).From(idlePosition + Vector2.right * 100).SetEase(Ease.OutQuad))
            .Join(canvasGroup.DOFade(1, 0.15f).From(0).SetEase(Ease.OutQuad));
    }

    public static Tween Hide(RectTransform target, Vector2 idlePosition)
    {
        return target.DOAnchorPos(idlePosition + Vector2.left * 100, 0.15f).From(idlePosition).SetEase(Ease.InQuad);
    }

    public static Tween Hide(RectTransform target, Vector2 idlePosition, CanvasGroup canvasGroup)
    {
        return DOTween.Sequence()
            .Append(target.DOAnchorPos(idlePosition + Vector2.left * 100, 0.15f).From(idlePosition).SetEase(Ease.InQuad))
            .Join(canvasGroup.DOFade(0, 0.15f).From(1).SetEase(Ease.InQuad));
    }
}
