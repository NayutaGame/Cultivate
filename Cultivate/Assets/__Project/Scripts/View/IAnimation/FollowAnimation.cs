
using DG.Tweening;
using UnityEngine;

public class FollowAnimation : FromCurrentAnimation
{
    private RectTransform Slot;
    private bool UseSlotRotation;
    private Configuration Configuration;
    private Ease Ease;
    private float Duration;

    private FollowAnimation(
        RectTransform content,
        RectTransform slot,
        bool useSlotRotation,
        Configuration configuration,
        Ease ease,
        float duration) : base(content)
    {
        Slot = slot;
        UseSlotRotation = useSlotRotation;
        Configuration = configuration;
        
        Ease = ease;
        Duration = duration;
    }

    public static FollowAnimation FromDefault(RectTransform content, RectTransform slot, bool useSlotRotation = false)
    {
        return new FollowAnimation(
            content,
            slot,
            useSlotRotation,
            Configuration.Default(),
            Ease.OutQuad,
            0.15f);
    }

    public static FollowAnimation FromConfiguration(RectTransform content, RectTransform slot, Configuration configuration)
    {
        return new FollowAnimation(
            content,
            slot,
            false,
            configuration,
            Ease.OutQuad,
            0.15f);
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease);
    }

    protected override void SetProgress(float t)
    {
        TryRecordConfiguration();
        
        Content.position = Vector3.Lerp(StartConfiguration.Position, Slot.position + Configuration.Position, t);
        Content.rotation = UseSlotRotation
            ? Quaternion.Slerp(StartConfiguration.Rotation, Slot.rotation * Configuration.Rotation, t)
            : Quaternion.Slerp(StartConfiguration.Rotation, Configuration.Rotation, t);
        Content.localScale = Vector3.Lerp(StartConfiguration.Scale, Configuration.Scale, t);
    }
}