
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Logic/Path Selector", -10, true)]
public class PathSelectorNode : Node, ILogicNode, ILogicConnector
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> PrevCell;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<int> Index;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode>[] Paths;

    public NodePort PrevPort => PrevCell;
    public NodePort NextPort 
    {
        get
        {
            if (Index.Value < 0 || Index.Value >= Paths.Length)
            {
                return Paths[0];
            }
            return Paths[Index.Value];
        }
    }

    public IEnumerable<ILogicNode> Prevs => PrevCell.Values;
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