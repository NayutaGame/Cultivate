using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Variable/EditorJingJie", -5, true)]
public class EditorJingJieNode : Node
{
    [PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<EditorJingJie> value;
}
