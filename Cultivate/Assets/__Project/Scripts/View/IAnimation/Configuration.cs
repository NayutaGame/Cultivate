
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct Configuration
{
    [FormerlySerializedAs("LocalPosition")] public Vector3 Position;
    public Quaternion Rotation;
    [FormerlySerializedAs("LocalScale")] public Vector3 Scale;

    public Configuration(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public static Configuration Default()
        => new(Vector3.zero, Quaternion.identity, Vector3.one);

    public static Configuration FromPosition(Vector3 position)
        => new(position, Quaternion.identity, Vector3.one);

    public static Configuration FromScale(Vector3 scale)
        => new(Vector3.zero, Quaternion.identity, scale);

    public static Configuration FromRect(RectTransform rect)
        => new(rect.position, rect.rotation, rect.localScale);
    
    // 乘法运算符：组合两个变换
    // T2 * T1 表示先应用 T1，再应用 T2
    // - 位置：T2.position + T2.rotation * (T2.scale * T1.position)
    // - 旋转：组合两个旋转（T2.rotation * T1.rotation）
    // - 缩放：逐分量相乘（T2.scale * T1.scale）
    public static Configuration operator *(Configuration t2, Configuration t1)
    {
        // 组合旋转
        Quaternion combinedRotation = t2.Rotation * t1.Rotation;
        
        // 组合缩放（逐分量相乘）
        Vector3 combinedScale = Vector3.Scale(t2.Scale, t1.Scale);
        
        // 组合位置：
        // T1 的位置先被 T2 的缩放缩放，然后被 T2 的旋转旋转，最后加上 T2 的位置
        Vector3 transformedPosition = t2.Position + t2.Rotation * Vector3.Scale(t2.Scale, t1.Position);
        
        return new Configuration(transformedPosition, combinedRotation, combinedScale);
    }
}
