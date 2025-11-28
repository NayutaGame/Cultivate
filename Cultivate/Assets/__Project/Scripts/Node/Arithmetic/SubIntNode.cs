
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Arithmetic/Sub Int", -10, true)]
[NodeHeaderTint(typeof(int))]
public class SubIntNode : Node
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> N1;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> N2;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, SpaceLine(-1)]
    private OutputPort<int> Value = new(self => (self as SubIntNode).GetValue());

    private int GetValue() => N1.Value - N2.Value;
}