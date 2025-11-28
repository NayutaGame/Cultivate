
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(200)]
[CreateNodeMenu("Cell/Barter Cell", -7, true)]
public class BarterCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> TargetItemCount = new(2);
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> TargetIsMutator = new(false);

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorRunSkillQuery> FromQuery;

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<EditorSkillEntryQuery> ToQuery;
    
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
        int targetItemCount = TargetItemCount.Value;
        bool targetIsMutator = TargetIsMutator.Value;
        RunSkillQuery fromQuery = RunSkillQuery.FromEditorQuery(FromQuery.Value);
        SkillEntryQuery toQuery = SkillEntryQuery.FromEditorQuery(ToQuery.Value);
        RunCostDefinition refreshCost = null; // Can be extended later if needed
        
        return BarterCell.FromEverything(
            targetItemCount: targetItemCount,
            targetIsMutator: targetIsMutator,
            fromQuery: fromQuery,
            toQuery: toQuery,
            refreshCost: refreshCost
        );
    }
    
    public override bool ReceiveSignal(Signal signal)
    {
        ExitShopSignal exitShopSignal = signal as ExitShopSignal;
        if (exitShopSignal == null)
            return false;
        
        return true;
    }
}
