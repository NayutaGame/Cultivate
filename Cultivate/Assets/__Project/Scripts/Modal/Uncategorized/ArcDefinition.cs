
using System;
using UnityEngine;

[Serializable]
public class ArcDefinition
{
    [SerializeField] public Vector2 Center;
    [SerializeField] public float Radius;
    [SerializeField] public float StartAngle;
    [SerializeField] public bool CW;
    [SerializeField] public float ArcAngle;
    
    [SerializeField] public float ArcLength;

    private ArcDefinition(Vector2 center, float radius, float startAngle, bool cw, float arcAngle)
    {
        Center = center;
        Radius = radius;
        StartAngle = startAngle;
        ArcAngle = arcAngle;
        CW = cw;

        ArcLength = GetArcLength();
    }

    public static ArcDefinition FromThreePoints(Vector2 startPoint, Vector2 middlePoint, Vector2 endPoint)
    {
        Vector2 p1 = startPoint;
        Vector2 p2 = middlePoint;
        Vector2 p3 = endPoint;
        
        // 检查边界情况：点重合
        Vector2 v1 = p2 - p1;
        Vector2 v2 = p3 - p2;
        float v1SqrMag = v1.sqrMagnitude;
        float v2SqrMag = v2.sqrMagnitude;
        
        const float epsilon = 1e-6f;
        if (v1SqrMag < epsilon || v2SqrMag < epsilon)
        {
            throw new ArgumentException("三个点中至少有两个点重合，无法形成圆弧");
        }
        
        // 检查边界情况：三点共线
        // 使用叉积判断：如果 v1 × v2 ≈ 0，则三点共线
        float cross = v1.x * v2.y - v1.y * v2.x;
        if (Mathf.Abs(cross) < epsilon)
        {
            throw new ArgumentException("三个点共线，无法形成圆弧");
        }
        
        // 计算归一化方向向量
        Vector2 p1p2 = v1.normalized;
        Vector2 p2p3 = v2.normalized;
        
        // 计算中垂线
        Vector2 midPoint1 = (p1 + p2) * 0.5f;
        Vector2 midPoint2 = (p2 + p3) * 0.5f;
        
        // 计算垂直向量（逆时针旋转90度）
        Vector2 perpDir1 = new Vector2(-p1p2.y, p1p2.x);
        Vector2 perpDir2 = new Vector2(-p2p3.y, p2p3.x);
        
        // 求圆心（两条中垂线的交点）
        float t = GetIntersectionParameter(midPoint1, perpDir1, midPoint2, perpDir2);
        Vector2 center = midPoint1 + perpDir1 * t;
        
        // 计算半径
        float radius = Vector2.Distance(center, p1);
        
        // 计算角度
        float startAngle = Mathf.Atan2(p1.y - center.y, p1.x - center.x);
        float endAngle = Mathf.Atan2(p3.y - center.y, p3.x - center.x);
        
        // 判断顺时针/逆时针
        // 如果叉积 < 0，说明从 p1->p2 到 p2->p3 是顺时针方向
        bool cw = cross < 0;
        
        // 计算弧角度
        // 使用 4π 确保结果在 [0, 2π] 范围内
        float arcAngle = cw
            ? -(startAngle - endAngle + 4 * Mathf.PI) % (2 * Mathf.PI)
            : (endAngle - startAngle + 4 * Mathf.PI) % (2 * Mathf.PI);
        
        return new ArcDefinition(center, radius, startAngle, cw, arcAngle);
    }
    
    private static float GetIntersectionParameter(Vector2 p1, Vector2 d1, Vector2 p2, Vector2 d2)
    {
        // 求两条直线的交点参数
        // 直线1: p1 + t * d1
        // 直线2: p2 + s * d2
        // 求 t 使得两条直线相交
        
        const float epsilon = 1e-6f;
        float denominator = d1.x * d2.y - d1.y * d2.x;
        
        if (Mathf.Abs(denominator) < epsilon)
        {
            throw new InvalidOperationException("两条中垂线平行或重合，无法计算圆心");
        }
        
        Vector2 dp = p2 - p1;
        return (dp.x * d2.y - dp.y * d2.x) / denominator;
    }
    
    public Vector2 EvaluatePoint(float t)
    {
        // t从0到1插值获取圆弧上的点
        float angle = Mathf.LerpUnclamped(StartAngle, StartAngle + ArcAngle, t);

        return new Vector2(
            Center.x + Radius * Mathf.Cos(angle),
            Center.y + Radius * Mathf.Sin(angle)
        );
    }
    
    public float EvaluateAngle(float t)
    {
        // 获取圆弧上点的切线角度（度），与 EvaluatePoint 保持一致
        float angle = Mathf.LerpUnclamped(StartAngle, StartAngle + ArcAngle, t);
        return angle * Mathf.Rad2Deg + 90f;
    }

    private float GetArcLength()
    {
        return Mathf.Abs(ArcAngle) * Radius;
    }
}
