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
    private InputPort<List<SkillEntryQuery>> DrawStrategies = new();
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<JingJieType> PreferredJingJie = new(JingJieType.LianQi);
    
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
            
        string title = Title.Value;
        string description = Description.Value;
        
        JingJieType preferredJingJieType = PreferredJingJie.Value;
        JingJie preferredJingJie = JingJie.FromJingJieType(preferredJingJieType);
        
        List<SkillEntryQuery> drawStrategies = DrawStrategies.Value ?? SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, preferredJingJie)).Stack(3);
        
        return DiscoverCell.FromEverything(
            titleText: title,
            descriptionText: description,
            drawStrategies: drawStrategies,
            preferredJingJie: preferredJingJie
        );
    }

    public override void ReceiveSignal(Signal signal)
    {
        if (signal is PickDiscoveredSkillSignal pickSignal)
        {
            int pickedIndex = pickSignal.Selected;
            // SkillEntryDescriptor skill = (_cell as DiscoverCell).GetSkills()[pickedIndex];
            // RunManager.Instance.Environment.PickDiscoveredSkillProcedure(pickedIndex, skill);
        }
    }
}
