using System.Collections.Generic;
using CLLibrary;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Discover Cell", -9, true)]
public class DiscoverCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Title = new("灵感");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Description = new("请选择一张卡作为奖励");

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<EditorSkillEntryQuery>> DrawStrategies;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<JingJieIndirect> PreferredJingJie = new();
    
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
        string title = Title.Value;
        string description = Description.Value;

        JingJie preferredJingJie = JingJie.FromIndirect(PreferredJingJie.Value);

        List<SkillEntryQuery> drawStrategies = SkillEntryQuery.FromEditorQueries(DrawStrategies.Value, preferredJingJie);

        return DiscoverCell.FromEverything(
            titleText: title,
            descriptionText: description,
            drawStrategies: drawStrategies,
            preferredJingJie: preferredJingJie
        );
    }

    public override bool ReceiveSignal(Signal signal)
    {
        if (signal is PickDiscoveredSkillSignal pickSignal)
        {
            int pickedIndex = pickSignal.Selected;
            // SkillEntryDescriptor skill = (_cell as DiscoverCell).GetSkills()[pickedIndex];
            // RunManager.Instance.Environment.PickDiscoveredSkillProcedure(pickedIndex, skill);
            return true;
        }

        return false;
    }
}
