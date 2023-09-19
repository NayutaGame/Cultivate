using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public interface IAnimation
{
    Tweener GetHandle();
    void SetProgress(float t);
}

public struct FollowAnimation : IAnimation
{
    public RectTransform Obj;
    public Vector2 StartPosition;
    public RectTransform Follow;

    public Tweener GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.15f);
    }

    public void SetProgress(float t)
    {
        Obj.anchoredPosition = Vector2.Lerp(StartPosition, Follow.anchoredPosition, t);
    }
}
