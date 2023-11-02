using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface IAnimation
{
    Tween GetHandle();
    void SetProgress(float t);
}

public struct FollowAnimationAnchored : IAnimation
{
    public RectTransform Obj;
    public Vector2 StartPosition;
    public RectTransform Follow;

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.15f);
    }

    public void SetProgress(float t)
    {
        Obj.anchoredPosition = Vector2.Lerp(StartPosition, Follow.anchoredPosition, t);
    }
}

public struct FollowAnimation : IAnimation
{
    private RectTransform Obj;
    private RectTransform Follow;

    private Vector2 StartPosition;
    private Vector2 StartScale;

    public FollowAnimation(RectTransform obj, RectTransform follow)
    {
        Obj = obj;
        Follow = follow;

        StartPosition = Obj.position;
        StartScale = Obj.localScale;
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.15f);
    }

    public void SetProgress(float t)
    {
        Obj.position = Vector2.Lerp(StartPosition, Follow.position, t);
        Obj.localScale = Vector2.Lerp(StartScale, Follow.localScale, t);
    }
}
