
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Cell/Dialog Cell Quick", -10, true)]
public class DialogCellNodeQuick : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [ArrowPort, PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField] [SpaceLine(-1)]
    private OutputPort<ILogicNode> NextCell = new(self => self as ILogicNode);
    
    [Multiline(5), PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DetailedText;
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => NextCell;
    
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
        var options = new List<DialogOption>();
        
        if (options.Count == 0)
            options.Add(DialogOption.FromText("继续"));
        
        return new DialogCell(
            "",
            DetailedText.Value,
            options.ToArray()
        );
    }

    public override bool ReceiveSignal(Signal signal)
    {
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            return true;
        }

        return false;
    }
}