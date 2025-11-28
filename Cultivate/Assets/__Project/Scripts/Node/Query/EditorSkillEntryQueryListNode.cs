
using System.Collections.Generic;
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Query/EditorSkillEntryQueryList", -10, true)]
public class EditorSkillEntryQueryListNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<List<EditorSkillEntryQuery>> value = new(self => (self as EditorSkillEntryQueryListNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private InputPort<EditorSkillEntryQuery>[] items;

    private List<EditorSkillEntryQuery> Build()
    {
        var list = new List<EditorSkillEntryQuery>();
        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var port = items[i];
                if (port == null)
                    continue;
                var v = port.Value;
                if (v != null)
                    list.Add(v);
            }
        }
        return list;
    }
}


