using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Variable/JingJieIndirect", -5, true)]
public class JingJieIndirectNode : Node
{
    [PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<JingJieIndirect> value;
}
