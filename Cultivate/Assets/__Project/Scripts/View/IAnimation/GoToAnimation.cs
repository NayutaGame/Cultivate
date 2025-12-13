
using DG.Tweening;
using UnityEngine;

public class GoToAnimation : CLAnimation
{
    private RectTransform Slot;

    private Configuration Configuration;
    
    private Ease Ease;
    private float Duration;

    private GoToAnimation(
        RectTransform content,
        RectTransform slot,
        Configuration configuration,
        Ease ease = Ease.OutQuad,
        float duration = 0.15f) : base(content)
    {
        Slot = slot;
        
        RecordConfiguration();

        Configuration = configuration;
        
        Ease = ease;
        Duration = duration;
    }

    public static GoToAnimation FromDefault(RectTransform content, RectTransform slot)
    {
        return new GoToAnimation(
            content,
            slot,
            Configuration.Default(),
            Ease.OutQuad,
            0.15f);
    }

    public static GoToAnimation FromConfiguration(RectTransform content, RectTransform slot, Configuration configuration)
    {
        return new GoToAnimation(
            content,
            slot,
            configuration,
            Ease.OutQuad,
            0.15f);
    }

    public static GoToAnimation FromPosition(RectTransform content, RectTransform slot, Vector3 position, Ease ease = Ease.OutQuad, float duration = 0.15f)
    {
        return new GoToAnimation(
            content,
            slot,
            Configuration.FromPosition(position),
            ease,
            duration);
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease);
    }

    protected override void SetProgress(float t)
    {
        Content.position = Vector3.Lerp(StartConfiguration.Position, Slot.position + Configuration.Position, t);
        Content.rotation = Quaternion.Slerp(StartConfiguration.Rotation, Configuration.Rotation, t);
        Content.localScale = Vector3.Lerp(StartConfiguration.Scale, Configuration.Scale, t);
    }
}