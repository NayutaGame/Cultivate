using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Variable/Query/EditorEntityQueryBuilder", -10, true)]
public class EditorEntityQueryBuilderNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<EditorEntityQuery> value = new(self => (self as EditorEntityQueryBuilderNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> entryName;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> ladder = new(-1);

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> targetDifficulty = new(-1);

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> limitToPool = new(true);

    private EditorEntityQuery Build()
    {
        var result = new EditorEntityQuery
        {
            EntryName = entryName.Value,
            Ladder = ladder.Value,
            TargetDifficulty = targetDifficulty.Value,
            LimitToPool = limitToPool.Value
        };
        return result;
    }
}
