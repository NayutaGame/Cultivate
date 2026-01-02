
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Arithmetic/Clamp Int", -10, true)]
[NodeHeaderTint(typeof(int))]
public class ClampIntNode : Node
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> Input;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> Upper;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> Lower;
    
    [PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField, SpaceLine(-1)]
    private OutputPort<int> Value = new(self => (self as ClampIntNode).GetValue());

    private int GetValue() => Mathf.Clamp(Input.Value, Upper.Value, Lower.Value);
}