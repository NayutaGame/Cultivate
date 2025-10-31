using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Variable/EditorTag", -5, true)]
public class EditorTagNode : Node
{
    [PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<EditorTag> value;
}
