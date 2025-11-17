
using CLLibrary;
using DG.Tweening;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Transform CameraMovementTransform;
    [SerializeField] private Camera Camera;
    [SerializeField] private ParallaxEffect ParallaxEffect;
    
    private Tween _cameraMovementHandle;

    public void CameraMovementAnimation(float degree)
    {
        // degree 为 -1 时，position.z = -7，rotation.y = -7
        // degree 为 0 时，position.z = -10，rotation.y = 0
        // degree 为 1 时，position.z = -7，rotation.y = 7
        
        // 限制 degree 在 [-1, 1] 范围内
        degree = Mathf.Clamp(degree, -1, 1);
        
        // 计算目标位置和旋转
        Vector3 targetPosition = CameraMovementTransform.position;
        Vector3 targetRotation = CameraMovementTransform.eulerAngles;
        
        // 根据 degree 计算目标 z 位置和 y 旋转
        float targetZ = Mathf.Lerp(-10f, -7f, Mathf.Abs(degree));
        float targetYRotation = degree * 7f; // degree 直接映射到 y 旋转
        
        targetPosition.z = targetZ;
        targetRotation.y = targetYRotation;
        
        _cameraMovementHandle?.Kill();
        _cameraMovementHandle = DOTween.Sequence()
            .Append(CameraMovementTransform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutCubic))
            .Join(CameraMovementTransform.DORotate(targetRotation, 0.5f).SetEase(Ease.OutCubic));
        _cameraMovementHandle.SetAutoKill().Restart();
    }

    public static Vector3 UI2World(Vector2 screenPosition)
    {
        return Instance.Camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 10));
    }

    public static Vector3 World2UI(Vector3 worldPosition)
    {
        return Instance.Camera.WorldToScreenPoint(worldPosition);
    }

    public static Vector3 ScreenCenterInWorld()
    {
        return Instance.Camera.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));
    }
    
    public Camera GetCamera()
        => Camera;

    public void SetParallaxEnabled(bool value)
    {
        ParallaxEffect.enabled = value;
    }

    public Vector3 ClampToScreenBounds(RectTransform rectTransform, Vector3 targetPosition)
    {
        Vector2 targetPosition_UI = World2UI(targetPosition);
        Vector2 screenSize_UI = new Vector2(Screen.width, Screen.height);
        
        // 获取RectTransform在屏幕上的实际边界
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        
        // 计算从当前位置到目标位置的偏移量
        Vector3 currentPosition = rectTransform.position;
        Vector3 offset = targetPosition - currentPosition;
        
        // 将世界坐标的四个角转换为屏幕坐标，并应用偏移量
        Vector2[] screenCorners = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            // 应用偏移量到corner位置
            Vector3 adjustedCorner = corners[i] + offset;
            screenCorners[i] = World2UI(adjustedCorner);
        }
        
        // 计算annotation在屏幕上的边界
        float minX = Mathf.Min(screenCorners[0].x, screenCorners[2].x);
        float maxX = Mathf.Max(screenCorners[0].x, screenCorners[2].x);
        float minY = Mathf.Min(screenCorners[0].y, screenCorners[2].y);
        float maxY = Mathf.Max(screenCorners[0].y, screenCorners[2].y);
        
        float annotationWidth = maxX - minX;
        float annotationHeight = maxY - minY;
        
        // 计算调整后的目标位置
        Vector2 adjustedPosition_UI = targetPosition_UI;
        
        // 调整X坐标
        if (minX < 0)
        {
            adjustedPosition_UI.x = targetPosition_UI.x - minX;
        }
        else if (maxX > screenSize_UI.x)
        {
            adjustedPosition_UI.x = targetPosition_UI.x - (maxX - screenSize_UI.x);
        }
        
        // 调整Y坐标
        if (minY < 0)
        {
            adjustedPosition_UI.y = targetPosition_UI.y - minY;
        }
        else if (maxY > screenSize_UI.y)
        {
            adjustedPosition_UI.y = targetPosition_UI.y - (maxY - screenSize_UI.y);
        }
        
        return UI2World(adjustedPosition_UI);
    }
}
