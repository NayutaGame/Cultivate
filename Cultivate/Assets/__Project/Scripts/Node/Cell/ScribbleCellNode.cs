
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Cell/Scribble Cell", -9, true)]
public class ScribbleCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> _next = new(self => self as ILogicNode);
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => _next;

    public override IEnumerable<ILogicNode> Prevs => prevs.Values;
    
    public override ILogicNode Next
    {
        get
        {
            var node = NextPort?.Connection?.Node as ILogicNode;
            while (node != null && node is ILogicConnector)
                node = node.Next;
            return node;
        }
    }
    
    public override void Execute()
    {
    }

    protected override Cell CreateInternalCell()
    {
        return new ScribbleCell();
    }

    public override bool ReceiveSignal(Signal signal)
    {
        return false;
    }
}