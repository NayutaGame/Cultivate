using PuppyDragon.uNody;
using UnityEngine;
using CLLibrary;

[NodeWidth(400)]
[CreateNodeMenu("Query/EditorSkillEntryQueryBuilder", -10, true)]
public class EditorSkillEntryQueryBuilderNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<EditorSkillEntryQuery> Value = new(self => (self as EditorSkillEntryQueryBuilderNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> EntryName;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorWuXing> WuXing;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorJingJie> LowBaseJingJie;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorJingJie> HighBaseJingJie;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorTag> Tag;

    private EditorSkillEntryQuery Build()
    {
        var result = new EditorSkillEntryQuery
        {
            EntryName = EntryName.Value,
            WuXing = WuXing.Value,
            LowBaseJingJie = LowBaseJingJie.Value,
            HighBaseJingJie = HighBaseJingJie.Value,
            Tag = Tag.Value
        };
        return result;
    }
}


