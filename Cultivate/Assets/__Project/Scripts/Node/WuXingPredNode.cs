
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Variable/WuXingPred", -5, true)]
public class WuXingPredNode : Node
{
    [PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<WuXingPred> value;
}