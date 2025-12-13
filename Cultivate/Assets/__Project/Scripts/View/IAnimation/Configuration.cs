
using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct Configuration
{
    [FormerlySerializedAs("LocalPosition")] public Vector3 Position;
    public Quaternion Rotation;
    [FormerlySerializedAs("LocalScale")] public Vector3 Scale;

    private Configuration(Vector3 position, Quaternion rotation, Vector3 scale)
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
}
