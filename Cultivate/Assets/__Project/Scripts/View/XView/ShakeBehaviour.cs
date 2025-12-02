
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(XView))]
public class ShakeBehaviour : XBehaviour
{
    [SerializeField] private InteractBehaviour _ib;
    
    private Vector3 _rotationAxisOffset = new Vector3(1, 1, 8).normalized;
    
    private Tween _handle;
    private RectTransform _rectTransform;
    private Quaternion _initialRotation; // 初始旋转
    private Vector3 _rotationAxis; // 旋转轴方向（世界坐标系）

    public override void AwakeFunction()
    {
        base.AwakeFunction();
        _rectTransform = GetView().GetRect();
        _initialRotation = _rectTransform.rotation;
    }

    private void OnEnable()
    {
        _ib.PointerEnterNeuron.Add(StartHoverRotation);
    }

    private void OnDisable()
    {
        _ib.PointerEnterNeuron.Remove(StartHoverRotation);
        _handle?.Kill();
        _rectTransform.rotation = _initialRotation;
    }

    public void StartHoverRotation(InteractBehaviour ib, PointerEventData d)
    {
        StartHoverRotation();
    }
    
    public void StartHoverRotation()
    {
        _handle?.Kill();
        
        // 计算旋转轴方向（在世界坐标系中）
        // RotationAxisOffset是相对于卡牌局部坐标系的偏移
        // 需要转换到世界坐标系
        Vector3 localAxis = _rotationAxisOffset;
        _rotationAxis = _rectTransform.TransformDirection(localAxis);
        
        // 保存初始旋转
        _initialRotation = Quaternion.identity;
        
        // 创建旋转动画
        _handle = DOTween.To(
            () => 0f,
            UpdateRotation,
            360f,
            .2f
        )
        .SetEase(Ease.Linear);
        
        _handle.SetAutoKill().Restart();
    }
    
    private void UpdateRotation(float angle)
    {
        // 1. 计算法线 n(t)：绕旋转轴旋转angle度
        // 初始法线是卡牌的forward方向（朝向屏幕外）
        Vector3 initialNormal = _initialRotation * Vector3.forward;
        Quaternion axisRotation = Quaternion.AngleAxis(angle, _rotationAxisOffset);
        Vector3 currentNormal = axisRotation * initialNormal;
        
        // 2. 计算θ(t)：绕法线的旋转角（相对于法线坐标系）
        float theta = -angle;
        
        // 3. 组合旋转：
        //    先绕轴旋转得到新的法线方向
        //    再绕新法线旋转θ，补偿翻滚
        Quaternion normalRotation = Quaternion.AngleAxis(theta, currentNormal);
        Quaternion finalRotation = normalRotation * axisRotation * _initialRotation;
        
        // 4. 应用旋转（保持位置不变）
        _rectTransform.rotation = finalRotation;
    }

    private void OnDestroy()
    {
        _ib.PointerEnterNeuron.Remove(StartHoverRotation);
        _handle?.Kill();
        _rectTransform.rotation = _initialRotation;
    }
}