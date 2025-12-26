
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Cell/Scribble Cell", -10, true)]
public class ScribbleCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> PrevCell;
    
    [ArrowPort, PortSettings(isHideLabel: true, ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    [SpaceLine(-1)]
    private OutputPort<ILogicNode> NextCell = new(self => self as ILogicNode);
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<CharacterEntry> CharacterEntry;
    
    public override NodePort PrevPort => PrevCell;
    public override NodePort NextPort => NextCell;

    public override IEnumerable<ILogicNode> Prevs => PrevCell.Values;
    
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
        return new ScribbleCell(CharacterEntry.Value);
    }

    public override bool ReceiveSignal(Signal signal)
    {
        ExitScribbleSignal exitScribbleSignal = signal as ExitScribbleSignal;
        if (exitScribbleSignal == null)
            return false;
        return true;
    }
}