using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Query/EditorRunSkillQuery", -10, true)]
public class EditorRunSkillQueryNode : Node
{
	[PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
	private OutputPort<EditorRunSkillQuery> value;
}


