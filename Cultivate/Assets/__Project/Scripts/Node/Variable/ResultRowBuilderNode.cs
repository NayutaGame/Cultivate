
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Variable/Result Row", -9, true)]
public class ResultRowBuilderNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ResultRow> value = new(self => (self as ResultRowBuilderNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Min;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Max;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> RewardDescription;

    private ResultRow Build()
    {
        if (!Application.isPlaying)
            return null;
        
        ResultRow resultRow = new ResultRow(
            Min.Value,
            Max.Value,
            RewardDescription.Value);
        return resultRow;
    }
}