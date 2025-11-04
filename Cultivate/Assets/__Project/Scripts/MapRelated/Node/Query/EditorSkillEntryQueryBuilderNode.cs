using PuppyDragon.uNody;
using UnityEngine;
using CLLibrary;

[NodeWidth(400)]
[CreateNodeMenu("Variable/Query/EditorSkillEntryQueryBuilder", -10, true)]
public class EditorSkillEntryQueryBuilderNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<EditorSkillEntryQuery> value = new(self => (self as EditorSkillEntryQueryBuilderNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> entryName;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorWuXing> wuXing;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<Bound> baseJingJieBound;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorTag> tag;

    private EditorSkillEntryQuery Build()
    {
        var result = new EditorSkillEntryQuery
        {
            EntryName = entryName.Value,
            WuXing = wuXing.Value,
            BaseJingJieBound = baseJingJieBound.Value,
            Tag = tag.Value
        };
        return result;
    }
}


