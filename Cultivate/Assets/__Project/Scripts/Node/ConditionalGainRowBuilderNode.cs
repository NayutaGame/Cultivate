
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Variable/Conditional Gain Row", -9, true)]
public class ConditionalGainRowBuilderNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ConditionalGainRow> value = new(self => (self as ConditionalGainRowBuilderNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> CondDescription;

    [PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> CurrentValue;

    [PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> Cond;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> ExtraCredit = new(0);

    private ConditionalGainRow Build()
    {
        if (!Application.isPlaying)
            return null;

        ConditionalGainRow gainRow = new ConditionalGainRow(
            CondDescription.Value,
            () => CurrentValue.Value,
            () => Cond.Value,
            ExtraCredit.Value);
        return gainRow;
    }
}