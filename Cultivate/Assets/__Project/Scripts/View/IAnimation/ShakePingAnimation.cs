
using DG.Tweening;
using UnityEngine;

public class ShakePingAnimation : IAnimation
{
    private RectTransform Slot;
    private RectTransform Content;
    
    private Vector3 StartPosition;
    private Quaternion StartRotation;
    private Vector3 StartScale;
    
    private Vector3 RotationAxis;
    private Vector3 TargetScale;
    private Vector3 EndScale;
    private float Duration;

    public ShakePingAnimation(RectTransform slot, RectTransform content, Vector3 targetScale, float duration)
    {
        Slot = slot;
        Content = content;
        
        RotationAxis = content.TransformDirection(new Vector3(1, 1, 8).normalized);
        TargetScale = targetScale;
        EndScale = Vector3.one;
        Duration = duration;
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, Duration).SetEase(Ease.Linear).OnPlay(RecordRigid);
    }

    private void RecordRigid()
    {
        StartPosition = Content.position;
        StartRotation = Content.rotation;
        StartScale = Content.localScale;
    }

    public void AppendHandle(Sequence seq)
    {
        seq.Append(GetHandle());
    }

    public void SetProgress(float t)
    {
        // Shake 部分：旋转动画
        float angle = t * 360;
        Vector3 initialNormal = StartRotation * Vector3.forward;
        Quaternion axisRotation = Quaternion.AngleAxis(angle, RotationAxis);
        Vector3 currentNormal = axisRotation * initialNormal;
        
        float theta = angle;
        Quaternion normalRotation = Quaternion.AngleAxis(theta, currentNormal);
        Quaternion finalRotation = normalRotation * axisRotation * StartRotation;
        
        Content.rotation = finalRotation;
        Content.position = StartPosition + new Vector3(0, 0, -0.5f);
        
        // Ping 部分：缩放动画
        if (t <= 0.5f)
        {
            // 第一阶段：从 StartScale 到 TargetScale
            float phaseT = t * 2f;
            Content.localScale = Vector3.Lerp(StartScale, TargetScale, EaseOutQuad(phaseT));
        }
        else
        {
            // 第二阶段：从 TargetScale 回到 EndScale
            float phaseT = (t - 0.5f) * 2f;
            Content.localScale = Vector3.Lerp(TargetScale, EndScale, EaseInQuad(phaseT));
        }
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