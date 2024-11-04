
using DG.Tweening;
using TMPro;
using UnityEngine;

public static class TweenAnimation
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
    
    public static Tween Show(RectTransform target, Vector2 idlePosition, TMP_Text text)
    {
        return DOTween.Sequence()
            .Append(target.DOAnchorPos(idlePosition, 0.15f).From(idlePosition + Vector2.right * 100).SetEase(Ease.OutQuad))
            .Join(text.DOFade(1, 0.15f).From(0).SetEase(Ease.OutQuad));
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

    public static Tween Hide(RectTransform target, Vector2 idlePosition, TMP_Text text)
    {
        return DOTween.Sequence()
            .Append(target.DOAnchorPos(idlePosition + Vector2.left * 100, 0.15f).From(idlePosition).SetEase(Ease.InQuad))
            .Join(text.DOFade(0, 0.15f).From(1).SetEase(Ease.InQuad));
    }

    public static Tween Beats(RectTransform target)
        => DOTween.Sequence()
            .Append(target.DOScale(0.8f, 0.1f).SetEase(Ease.InQuad))
            .Append(target.DOScale(1.5f, 0.1f).SetEase(Ease.Linear))
            .Append(target.DOScale(1f, 0.2f).SetEase(Ease.OutQuad))
            .AppendInterval(0.6f)
            .SetLoops(-1, loopType: LoopType.Restart);

    public static Tween Jump(RectTransform target)
        => target.DOScale(Vector3.one, 0.7f).From(1.2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad);
}
