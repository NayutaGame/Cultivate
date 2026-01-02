
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[NodeHeaderTint(typeof(bool))]
public abstract class PredNode : Node
{
    [PortSettings(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<bool> Result = new(self =>
    {
        if (!Application.isPlaying)
            return false;
        return (self as PredNode).GetResult();
    });

    public abstract bool GetResult();
}