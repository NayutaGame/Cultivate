
using PuppyDragon.uNody;
using Sirenix.OdinInspector;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Query/EditorSkillEntryQuery", -10, true)]
public class EditorSkillEntryQueryNode : Node
{
    [PortSettings(isHideLabel: true, ShowBackingValue.Always, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<EditorSkillEntryQuery> Value;
}
