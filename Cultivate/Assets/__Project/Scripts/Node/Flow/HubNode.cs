
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Logic/Hub", -10, true)]
public class HubNode : Node, ILogicNode, ILogicConnector
{
    [ArrowPort, PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> NextCell;
    
    [ArrowPort, PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode>[] PrevCells;

    public NodePort PrevPort => PrevCells[0];
    public NodePort NextPort => NextCell;

    public IEnumerable<ILogicNode> Prevs => PrevCells[0].Values;
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