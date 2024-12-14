
using System;
using UnityEngine;

[Serializable]
public struct Configuration
{
    public Vector3 LocalPosition;
    public Vector3 LocalScale;

    public Configuration(Vector3? localPosition = null, Vector3? localScale = null)
    {
        LocalPosition = localPosition ?? Vector3.zero;
        LocalScale = localScale ?? Vector3.one;
    }
}
