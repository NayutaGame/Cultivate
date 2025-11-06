
using PuppyDragon.uNody;
using UnityEngine;
    
[NodeWidth(NodeSize.Small)]
[NodeHeaderTint(typeof(bool))]
[CreateNodeMenu("Variable/Bool", -6, true)]
public class BoolNode : Node
{
    [SerializeField]
    [PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)]
    private OutputPort<bool> value;
}