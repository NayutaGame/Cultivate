using PuppyDragon.uNody;
using UnityEngine;
using CLLibrary;

[NodeWidth(400)]
[CreateNodeMenu("Variable/Query/EditorRunSkillQueryBuilder", -10, true)]
public class EditorRunSkillQueryBuilderNode : Node
{
	[PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
	private OutputPort<EditorRunSkillQuery> value = new(self => (self as EditorRunSkillQueryBuilderNode).Build());

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<string> entryName;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<EditorWuXing> wuXing;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<EditorJingJie> jingJie;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<Bound> baseJingJieBound;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<EditorTag> tag;

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
	private InputPort<string> description;

	private EditorRunSkillQuery Build()
	{
		var result = new EditorRunSkillQuery
		{
			EntryName = entryName.Value,
			WuXing = wuXing.Value,
			JingJie = jingJie.Value,
			BaseJingJieBound = baseJingJieBound.Value,
			Tag = tag.Value,
			Description = description.Value
		};
		return result;
	}
}