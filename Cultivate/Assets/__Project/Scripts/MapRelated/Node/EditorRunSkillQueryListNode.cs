using System.Collections.Generic;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/EditorRunSkillQueryList", -3, true)]
public class EditorRunSkillQueryListNode : Node
{
	[PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
	private OutputPort<List<EditorRunSkillQuery>> value = new(self => (self as EditorRunSkillQueryListNode).Build());

	[PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
	private InputPort<EditorRunSkillQuery>[] items;

	private List<EditorRunSkillQuery> Build()
	{
		var list = new List<EditorRunSkillQuery>();
		if (items != null)
		{
			for (int i = 0; i < items.Length; i++)
			{
				var port = items[i];
				if (port == null) continue;
				var v = port.Value;
				if (v != null)
					list.Add(v);
			}
		}
		return list;
	}
}