
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float parallaxStrengthX = 0.1f; // X轴视差强度
    [SerializeField] private float parallaxStrengthY = 0.1f; // Y轴视差强度
    [SerializeField] private float smoothTime = 0.1f; // 平滑时间
    
    [Header("Axis Control")]
    [SerializeField] private bool enableXAxis = true; // 启用X轴视差
    [SerializeField] private bool enableYAxis = true; // 启用Y轴视差
    
    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private Vector3 initialPosition;
    
    private void Start()
    {
        // 记录初始位置
        initialPosition = transform.localPosition;
    }
    
    private void Update()
    {
        // 获取鼠标位置并进行clamp
        Vector2 mousePos = new Vector2(
            Mathf.Clamp(Input.mousePosition.x, 0f, Screen.width),
            Mathf.Clamp(Input.mousePosition.y, 0f, Screen.height)
        );
        
        // 计算与屏幕中心的偏移
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 offset = -(mousePos - screenCenter) / screenCenter; // 归一化到[-1, 1]范围
        
        // 计算目标位置（基于初始位置，分别处理x和y分量）
        float targetX = enableXAxis ? offset.x * parallaxStrengthX : 0f;
        float targetY = enableYAxis ? offset.y * parallaxStrengthY : 0f;
        
        targetPosition = initialPosition + new Vector3(targetX, targetY, 0);
        
        // 平滑移动到目标位置（保持z分量不变）
        Vector3 currentPos = transform.localPosition;
        Vector3 smoothedPos = Vector3.SmoothDamp(
            currentPos,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );
        
        // 保持原始的z分量
        transform.localPosition = new Vector3(smoothedPos.x, smoothedPos.y, currentPos.z);
    }
}