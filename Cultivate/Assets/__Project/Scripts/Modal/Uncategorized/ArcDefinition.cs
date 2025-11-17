
using UnityEngine;

public class ArcDefinition
{
    public Vector2 Center { get; private set; }
    public float Radius { get; private set; }
    public float StartAngle { get; private set; }
    public float EndAngle { get; private set; }

    private ArcDefinition(Vector2 center, float radius, float startAngle, float endAngle)
    {
        Center = center;
        Radius = radius;
        StartAngle = startAngle;
        EndAngle = endAngle;
    }

    public static ArcDefinition FromThreePoints(Vector2 startPoint, Vector2 middlePoint, Vector2 endPoint)
    {
        Vector2 p1 = startPoint;
        Vector2 p2 = middlePoint;
        Vector2 p3 = endPoint;
        
        // 1. 求两条中垂线
        Vector2 midPoint1 = (p1 + p2) * 0.5f;
        Vector2 midPoint2 = (p2 + p3) * 0.5f;
        
        // 2. 计算垂直向量
        Vector2 dir1 = (p2 - p1).normalized;
        Vector2 dir2 = (p3 - p2).normalized;
        Vector2 perpDir1 = new Vector2(-dir1.y, dir1.x);
        Vector2 perpDir2 = new Vector2(-dir2.y, dir2.x);
        
        // 3. 求圆心（两条中垂线的交点）
        float t = GetIntersectionParameter(
            midPoint1, perpDir1,
            midPoint2, perpDir2
        );
        
        Vector2 center = midPoint1 + perpDir1 * t;
        
        // 4. 计算半径
        float radius = Vector2.Distance(center, p1);
        
        // 5. 计算起始和结束角度
        float startAngle = Mathf.Atan2(p1.y - center.y, p1.x - center.x);
        float endAngle = Mathf.Atan2(p3.y - center.y, p3.x - center.x);
        
        // 确保角度是按照中间点的方向
        float midAngle = Mathf.Atan2(p2.y - center.y, p2.x - center.x);
        if (!IsAngleBetween(midAngle, startAngle, endAngle))
        {
            // 交换起始和结束角度
            (startAngle, endAngle) = (endAngle, startAngle);
        }
        
        return new ArcDefinition(center, radius, startAngle, endAngle);
    }
    
    private static float GetIntersectionParameter(Vector2 p1, Vector2 d1, Vector2 p2, Vector2 d2)
    {
        // 求两条直线的交点参数
        float denominator = d1.x * d2.y - d1.y * d2.x;
        if (Mathf.Abs(denominator) < 1e-6f)
            return 0f; // 平行或重合
            
        Vector2 dp = p2 - p1;
        return (dp.x * d2.y - dp.y * d2.x) / denominator;
    }
    
    private static bool IsAngleBetween(float angle, float start, float end)
    {
        // 标准化角度到 [-π, π]
        float normAngle = NormalizeAngle(angle - start);
        float normEnd = NormalizeAngle(end - start);
        
        return normEnd > 0 
            ? normAngle >= 0 && normAngle <= normEnd
            : normAngle >= normEnd && normAngle <= 0;
    }
    
    private static float NormalizeAngle(float angle)
    {
        // 将角度标准化到 [-π, π]
        angle = angle % (2 * Mathf.PI);
        if (angle > Mathf.PI)
            angle -= 2 * Mathf.PI;
        else if (angle < -Mathf.PI)
            angle += 2 * Mathf.PI;
        return angle;
    }
    
    public Vector2 EvaluatePoint(float t)
    {
        // t从0到1插值获取圆弧上的点
        // float angle = Mathf.Lerp(StartAngle, EndAngle, t);
        
        float angle = Mathf.LerpUnclamped(StartAngle, EndAngle, t);

        return new Vector2(
            Center.x + Radius * Mathf.Cos(angle),
            Center.y + Radius * Mathf.Sin(angle)
        );
    }
    
    public float EvaluateAngle(float t)
    {
        // 获取圆弧上点的切线角度
        // return Mathf.Lerp(StartAngle, EndAngle, t) * Mathf.Rad2Deg + 90f;
        return Mathf.LerpUnclamped(StartAngle, EndAngle, t) * Mathf.Rad2Deg + 90f;
    }

    public float GetArcLength()
    {
        return Mathf.Abs(EndAngle - StartAngle) * Radius;
    }
}
