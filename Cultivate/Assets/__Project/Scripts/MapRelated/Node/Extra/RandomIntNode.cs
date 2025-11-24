
using PuppyDragon.uNody;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu(true)]
[NodeHeaderTint(typeof(int))]
public class RandomIntNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<int> Start;
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<int> End;
    
    [SerializeField]
    private OutputPort<int> result = new(self => (self as RandomIntNode).Result);

    public int Result => Random.Range(Start.Value, End.Value);
}