
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Discover Cell From Ladder", -9, true)]
public class DiscoverCellNodeFromLadder : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<bool> IsWin;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Ladder;
    
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
        string title;
        string description;
        int goldValue = 0;

        if (IsWin.Value)
        {
            title = "胜利";
            description = $"获得了<style=\"Gold\">{goldValue}金钱</style>\n请选择<style=\"Red\">一张卡牌</style>作为奖励";
        }
        else
        {
            title = "惜败";
            description = $"<style=\"Gray\">你没能击败对手，损失了一些命元</style>" +
                          $"\n但获得了<style=\"Gold\">{goldValue}金钱</style>，以及选择<style=\"Red\">一张卡牌</style>作为奖励";
        }

        JingJie preferredJingJie = RoomDefinition.GetJingJieFromLadder(Ladder.Value);
        List<SkillEntryQuery> drawStrategies = SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, preferredJingJie)).Stack(3);

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
