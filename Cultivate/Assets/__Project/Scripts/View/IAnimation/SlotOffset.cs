
using UnityEngine;

public struct SlotOffset
{
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public SlotOffset(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
        
        // Mat4x4
    }

    public static SlotOffset Default()
        => new(Vector3.zero, Quaternion.identity, Vector3.one);

    public static SlotOffset FromPosition(Vector3 position)
        => new(position, Quaternion.identity, Vector3.one);
}