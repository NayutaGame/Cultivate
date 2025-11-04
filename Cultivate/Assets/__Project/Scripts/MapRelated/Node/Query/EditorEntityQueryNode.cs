using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/Query/EditorEntityQuery", -10, true)]
public class EditorEntityQueryNode : Node
{
    [PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<EditorEntityQuery> value;
}
