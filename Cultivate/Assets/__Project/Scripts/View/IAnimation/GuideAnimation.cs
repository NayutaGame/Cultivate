
using DG.Tweening;
using UnityEngine;

public class GuideAnimation : CLAnimation
{
    private RectTransform Start;
    private RectTransform End;

    public GuideAnimation(RectTransform content, RectTransform start, RectTransform end) : base(content)
    {
        Start = start;
        End = end;
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 1.5f).SetEase(Ease.InOutQuad);
    }

    protected override void SetProgress(float t)
    {
        Content.position = Vector3.Lerp(Start.position, End.position, t);
        Content.rotation = Quaternion.Slerp(Start.rotation, End.rotation, t);
        Content.localScale = Vector3.Lerp(Start.localScale, End.localScale, t);
    }
}