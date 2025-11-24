
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

public abstract class ProcedureNode : Node, ILogicNode, ILogicConnector
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)]
    [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> next = new(self => self as ILogicNode);

    public NodePort PrevPort => prevs;
    public NodePort NextPort => next;

    public IEnumerable<ILogicNode> Prevs => prevs.Values;
    public ILogicNode Next
    {
        get
        {
            var node = NextPort.Connection?.Node as ILogicNode;
            Procedure();
            while (node != null && node is ILogicConnector)
                node = node.Next;
            return node;
        }
    }

    public void Execute()
    {
    }

    public abstract void Procedure();
}