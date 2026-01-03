
using PuppyDragon.uNody;
using UnityEngine;

public class BattleOutcomeStringNode : Node
{
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<bool> IsWin;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited), SerializeField]
    private InputPort<int> Gold;
    
    [PortSettings(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited), SerializeField]
    private OutputPort<string> String = new(self => (self as BattleOutcomeStringNode).GetValue());

    private string GetValue()
    {
        if (IsWin.Value)
        {
            return $"获得了<style=\"Gold\">{Gold.Value}金钱</style>\n请选择<style=\"Red\">一张卡牌</style>作为奖励";
        }
        else
        {
            return $"<style=\"Gray\">你没能击败对手，损失了一些命元</style>\n但获得了<style=\"Gold\">{Gold.Value}金钱</style>，以及选择<style=\"Red\">一张卡牌</style>作为奖励";
        }
    }
}