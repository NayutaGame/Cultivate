using DG.Tweening;
using UnityEngine;

public class ShakeAnimation : IAnimation
{
    private RectTransform Slot;
    private RectTransform Content;

    private Vector3 StartPosition;
    private Quaternion StartRotation;
    private Vector3 StartScale;

    private Vector3 RotationAxis;

    public ShakeAnimation(RectTransform slot, RectTransform content)
    {
        Slot = slot;
        Content = content;
        
        StartPosition = content.position;
        StartRotation = content.rotation;
        StartScale = content.localScale;
        
        RotationAxis = content.TransformDirection(new Vector3(1, 1, 8).normalized);
    }

    public Tween GetHandle()
    {
        return DOTween.To(SetProgress, 0, 1, 0.2f).SetEase(Ease.Linear);
    }

    public void AppendHandle(Sequence seq)
    {
        seq.Append(GetHandle());
    }

    public void SetProgress(float t)
    {
        float angle = t * 360;
        // 1. 计算法线 n(t)：绕旋转轴旋转angle度
        Vector3 initialNormal = StartRotation * Vector3.forward;
        Quaternion axisRotation = Quaternion.AngleAxis(angle, RotationAxis);
        Vector3 currentNormal = axisRotation * initialNormal;
        
        // 2. 计算θ(t)：绕法线的旋转角（相对于法线坐标系）
        float theta = -angle;
        
        // 3. 组合旋转：
        //    先绕轴旋转得到新的法线方向
        //    再绕新法线旋转θ，补偿翻滚
        Quaternion normalRotation = Quaternion.AngleAxis(theta, currentNormal);
        Quaternion finalRotation = normalRotation * axisRotation * StartRotation;
        
        // 4. 应用旋转（保持位置不变）
        Content.rotation = finalRotation;
        Content.position = StartPosition + new Vector3(0, 0, -0.5f);
    }
}