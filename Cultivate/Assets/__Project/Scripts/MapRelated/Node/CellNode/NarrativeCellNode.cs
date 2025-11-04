
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Narrative Cell", -9, true)]
public class NarrativeCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [SerializeReference]
    private List<Commend> Commends;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> next = new(self => self as ILogicNode);
    
    protected override void Initialize()
    {
    }
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => next;
    
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
        if (!Application.isPlaying)
            return null;
        
        return new NarrativeCell(Commends ?? new List<Commend>());
    }

    public override bool ReceiveSignal(Signal signal)
    {
        if (signal is ProcessNarrativeSignal processNarrativeSignal)
        {
            NarrativeCell c = AsCell() as NarrativeCell;
            if (!c.HasNextCommend())
            {
                return true;
            }
            else
            {
                (AsCell() as NarrativeCell).ProcessCommend();
                return false;
            }
        }

        return false;
    }
}