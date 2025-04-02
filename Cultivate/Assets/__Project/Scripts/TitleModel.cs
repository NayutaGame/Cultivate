
using UnityEngine;

public class TitleModel : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float parallaxStrength = 0.1f; // 视差强度
    [SerializeField] private float smoothTime = 0.1f; // 平滑时间
    
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
        // 获取鼠标位置
        Vector2 mousePos = Input.mousePosition;
        
        // 计算与屏幕中心的偏移
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 offset = -(mousePos - screenCenter) / screenCenter; // 归一化到[-1, 1]范围
        
        // 计算目标位置（基于初始位置）
        targetPosition = initialPosition + new Vector3(
            offset.x * parallaxStrength,
            offset.y * parallaxStrength,
            0
        );
        
        // 平滑移动到目标位置
        transform.localPosition = Vector3.SmoothDamp(
            transform.localPosition,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );
    }
}