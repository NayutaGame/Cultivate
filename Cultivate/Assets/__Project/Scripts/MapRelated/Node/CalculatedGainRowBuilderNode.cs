
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Variable/Calculated Gain Row", -9, true)]
public class CalculatedGainRowBuilderNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<CalculatedGainRow> value = new(self => (self as CalculatedGainRowBuilderNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> PointDescription;

    [PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> CurrentCount;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> ExtraCreditPerPoint = new(0);

    private CalculatedGainRow Build()
    {
        if (!Application.isPlaying)
            return null;

        CalculatedGainRow gainRow = new CalculatedGainRow(
            PointDescription.Value,
            () => CurrentCount.Value,
            ExtraCreditPerPoint.Value);
        return gainRow;
    }
}