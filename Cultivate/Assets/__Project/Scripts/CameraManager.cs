
using CLLibrary;
using DG.Tweening;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Transform CameraMovementTransform;
    [SerializeField] private Camera Camera;
    
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
}
