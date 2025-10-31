using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/EditorRunSkillQuery", -5, true)]
public class EditorRunSkillQueryNode : Node
{
	[PortSettings(true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
	private OutputPort<EditorRunSkillQuery> value;
}


