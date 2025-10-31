using PuppyDragon.uNody;
using UnityEngine;
using CLLibrary;

[NodeWidth(NodeSize.Medium)]
[CreateNodeMenu("Variable/Bound", -5, true)]
public class BoundNode : Node
{
    [PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<Bound> value = new(self => (self as BoundNode).Build());

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> start = new(0);

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> end = new(1);

    private Bound Build()
    {
        return new Bound(start.Value, end.Value);
    }
}
