
using DG.Tweening;
using UnityEngine;

public class ShakePingAnimation : FromCurrentAnimation
{
    private Vector3 RotationAxis;
    private float ZChange;
    private Vector3 TargetScale;
    private Vector3 EndScale;
    private float Duration;

    public ShakePingAnimation(RectTransform content, float zChange, Vector3 targetScale, float duration) : base(content)
    {
        RotationAxis = content.TransformDirection(new Vector3(1, 1, 8).normalized);
        ZChange = zChange;
        TargetScale = targetScale;
        EndScale = Vector3.one;
        Duration = duration;
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease.Linear);
    }

    protected override void SetProgress(float t)
    {
        TryRecordConfiguration();
        
        float blend = Mathf.Clamp01(t * 3);
        
        // Shake 部分：旋转动画
        float angle = t * 360;
        Vector3 initialNormal = StartConfiguration.Rotation * Vector3.forward;
        Quaternion axisRotation = Quaternion.AngleAxis(angle, RotationAxis);
        Vector3 currentNormal = axisRotation * initialNormal;
        
        float theta = -angle;
        Quaternion normalRotation = Quaternion.AngleAxis(theta, currentNormal);
        Quaternion finalRotation = normalRotation * axisRotation * StartConfiguration.Rotation;
        
        // Ping 部分：缩放动画
        Vector3 finalScale;
        if (t <= 0.5f)
        {
            // 第一阶段：从 StartScale 到 TargetScale
            float phaseT = t * 2f;
            finalScale = Vector3.Lerp(StartConfiguration.Scale, TargetScale, EaseOutQuad(phaseT));
        }
        else
        {
            // 第二阶段：从 TargetScale 回到 EndScale
            float phaseT = (t - 0.5f) * 2f;
            finalScale = Vector3.Lerp(TargetScale, EndScale, EaseInQuad(phaseT));
        }
        
        Content.position = Vector3.Lerp(StartConfiguration.Position, StartConfiguration.Position + new Vector3(0, 0, ZChange), blend);
        Content.rotation = Quaternion.Slerp(StartConfiguration.Rotation, finalRotation, blend);
        Content.localScale = finalScale;
    }

    private static float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    private static float EaseInQuad(float t)
    {
        return t * t;
    }
}