using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(NodeSize.Small)]
[CreateNodeMenu("Logic/Switch", -8, true)]
public class SwitchNode : Node, ILogicNode, ILogicConnector
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<int> pathIndex;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode>[] Paths;

    public NodePort PrevPort => prevs;
    public NodePort NextPort 
    {
        get
        {
            if (pathIndex.Value < 0 || pathIndex.Value >= Paths.Length)
                return null;
            return Paths[pathIndex.Value];
        }
    }

    public IEnumerable<ILogicNode> Prevs => prevs.Values;
    public ILogicNode Next
    {
        get
        {
            var node = NextPort.Connection?.Node as ILogicNode;
            while (node != null && node is ILogicConnector)
                node = node.Next;
            return node;
        }
    }

    public void Execute()
    {
    }
}