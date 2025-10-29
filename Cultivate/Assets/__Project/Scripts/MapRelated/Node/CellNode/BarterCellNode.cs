
using System;
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(400)]
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
    private InputPort<bool> HasRefreshCost = new(false);
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> RefreshCostSkillName = new();
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> RefreshCostDescription = new();
    
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
        if (!Application.isPlaying)
            return null;
            
        // int targetItemCount = TargetItemCount.Value;
        // bool targetIsMutator = TargetIsMutator.Value;
        //
        // Predicate<RunSkill> fromPred = null; // Can be extended later
        // Predicate<SkillEntry> toPred = null; // Can be extended later
        //
        // RunCostDefinition refreshCost = null;
        // if (HasRefreshCost.Value && !string.IsNullOrEmpty(RefreshCostSkillName.Value))
        // {
        //     refreshCost = new SkillCostDefinition(
        //         RunSkillDescriptor.FromName(RefreshCostSkillName.Value),
        //         RefreshCostDescription.Value
        //     );
        // }
        //
        // return BarterCell.FromEverything(targetItemCount, targetIsMutator, fromPred, toPred, refreshCost);
        return null;
    }
    
    public override void ReceiveSignal(Signal signal)
    {
        ExitShopSignal exitShopSignal = signal as ExitShopSignal;
        if (exitShopSignal == null)
            return;
    }
}
