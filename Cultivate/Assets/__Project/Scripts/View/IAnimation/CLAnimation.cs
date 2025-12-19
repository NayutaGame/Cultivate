
using DG.Tweening;
using UnityEngine;

public abstract class CLAnimation
{
    protected RectTransform Content;
    
    public CLAnimation(RectTransform content)
    {
        Content = content;
    }
    
    public abstract Tween GetHandle();
    
    protected abstract void SetProgress(float t);
}