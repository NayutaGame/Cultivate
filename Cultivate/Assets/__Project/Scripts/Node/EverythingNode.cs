
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[NodeHeaderTint(typeof(string))]
[CreateNodeMenu("Cell/Require Cell", -10, true)]
public abstract class EverythingNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> A;
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField]
    private OutputPort<int> B = new(self => (self as EverythingNode).GetValue());

    private int GetValue() => 0;
}