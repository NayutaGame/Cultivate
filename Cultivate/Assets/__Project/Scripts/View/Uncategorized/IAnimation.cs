
using DG.Tweening;
using UnityEngine;

public interface IAnimation
{
    Tween GetHandle();
    void SetProgress(float t);
}

public struct FollowAnimation : IAnimation
{
    private RectTransform Subject;
    private RectTransform Follow;

    private Vector2 StartPosition;
    private Vector2 StartScale;

    public FollowAnimation(RectTransform subject, RectTransform follow)
    {
        Subject = subject;
        Follow = follow;

        StartPosition = Subject.position;
        StartScale = Subject.localScale;
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.15f).SetEase(Ease.OutQuad);
    }

    public void SetProgress(float t)
    {
        Subject.position = Vector2.Lerp(StartPosition, Follow.position, t);
        Subject.localScale = Vector2.Lerp(StartScale, Follow.localScale, t);
    }
}

public struct GuideAnimation : IAnimation
{
    private RectTransform Start;
    private RectTransform End;

    private RectTransform Subject;

    public GuideAnimation(RectTransform subject, RectTransform start, RectTransform end)
    {
        Subject = subject;
        Start = start;
        End = end;
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 1.5f).SetEase(Ease.InOutQuad);
    }

    public void SetProgress(float t)
    {
        Subject.position = Vector2.Lerp(Start.position, End.position, t);
    }
}
