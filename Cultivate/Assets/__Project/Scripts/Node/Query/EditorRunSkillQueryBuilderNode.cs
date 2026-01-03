using PuppyDragon.uNody;
using UnityEngine;
using CLLibrary;

[NodeWidth(400)]
[CreateNodeMenu("Query/EditorRunSkillQueryBuilder", -10, true)]
public class EditorRunSkillQueryBuilderNode : Node
{
	[PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
	private OutputPort<EditorRunSkillQuery> Value = new(self => (self as EditorRunSkillQueryBuilderNode).Build());

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<string> EntryName;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<WuXingPred> WuXingPred;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<JingJiePred> JingJiePred;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<JingJieIndirect> LowBaseJingJie;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<JingJieIndirect> HighBaseJingJie;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<EditorTag> Tag;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<string> Description;
	
	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	public InputPort<int> AttackRequirement;
	
	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	public InputPort<int> ArmorRequirement;

	private EditorRunSkillQuery Build()
	{
		if (!Application.isPlaying)
			return null;

		var result = new EditorRunSkillQuery
		{
			EntryName = EntryName.Value,
			WuXingPred = WuXingPred.Value,
			JingJiePred = JingJiePred.Value,
			LowBaseJingJie = LowBaseJingJie.Value,
			HighBaseJingJie = HighBaseJingJie.Value,
			Tag = Tag.Value,
			Description = Description.Value,
			AttackRequirement = AttackRequirement.Value,
			ArmorRequirement = ArmorRequirement.Value
		};
		return result;
	}
}