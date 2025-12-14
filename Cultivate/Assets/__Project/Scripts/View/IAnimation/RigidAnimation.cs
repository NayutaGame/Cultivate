
using DG.Tweening;
using UnityEngine;

public class RigidAnimation : CLAnimation
{
    private Configuration Configuration;
    private bool IsRelative;
    private Ease Ease;
    private float Duration;

    private Configuration TargetConfiguration;

    private RigidAnimation(
        RectTransform content,
        Configuration configuration,
        bool isRelative,
        Ease ease,
        float duration) : base(content)
    {
        Configuration = configuration;
        IsRelative = isRelative;
        Ease = ease;
        Duration = duration;
    }

    public static RigidAnimation FromRelative(RectTransform content, Configuration configuration, Ease ease = Ease.OutQuad, float duration = 0.15f)
    {
        return new RigidAnimation(
            content,
            configuration,
            true,
            ease,
            duration);
    }

    public static RigidAnimation FromAbsolute(RectTransform content, Configuration configuration, Ease ease = Ease.OutQuad, float duration = 0.15f)
    {
        return new RigidAnimation(
            content,
            configuration,
            false,
            ease,
            duration);
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease).OnPlay(RecordConfiguration);
    }

    protected override void SetProgress(float t)
    {
        Content.position = Vector3.Lerp(StartConfiguration.Position, TargetConfiguration.Position, t);
        Content.rotation = Quaternion.Slerp(StartConfiguration.Rotation, TargetConfiguration.Rotation, t);
        Content.localScale = Vector3.Lerp(StartConfiguration.Scale, TargetConfiguration.Scale, t);
    }

    public override void RecordConfiguration()
    {
        base.RecordConfiguration();

        if (IsRelative)
        {
            TargetConfiguration = Configuration * StartConfiguration;
        }
        else
        {
            TargetConfiguration = Configuration;
        }
    }
}