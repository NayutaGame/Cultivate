using DG.Tweening;
using UnityEngine;

public class ShakeAnimation : CLAnimation
{
    private RectTransform Slot;

    private Vector3 RotationAxis;

    public ShakeAnimation(RectTransform content, RectTransform slot) : base(content)
    {
        Slot = slot;
        
        RecordConfiguration();
        
        RotationAxis = content.TransformDirection(new Vector3(1, 1, 8).normalized);
    }

    public override Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.2f).SetEase(Ease.Linear);
    }

    protected override void SetProgress(float t)
    {
        float blend = Mathf.Clamp01(t * 3);
        
        float angle = t * 360;
        // 1. 计算法线 n(t)：绕旋转轴旋转angle度
        Vector3 initialNormal = StartConfiguration.Rotation * Vector3.forward;
        Quaternion axisRotation = Quaternion.AngleAxis(angle, RotationAxis);
        Vector3 currentNormal = axisRotation * initialNormal;
        
        // 2. 计算θ(t)：绕法线的旋转角（相对于法线坐标系）
        float theta = -angle;
        
        // 3. 组合旋转：
        //    先绕轴旋转得到新的法线方向
        //    再绕新法线旋转θ，补偿翻滚
        Quaternion normalRotation = Quaternion.AngleAxis(theta, currentNormal);
        Quaternion finalRotation = normalRotation * axisRotation * StartConfiguration.Rotation;
    
        Content.position = Vector3.Lerp(StartConfiguration.Position, Slot.position + new Vector3(0, 0, -0.5f), blend);
        Content.rotation = Quaternion.Slerp(StartConfiguration.Rotation, finalRotation, blend);
    }
}