
// 作为耗材：
// 提交一张二动牌
// 提交一张治疗牌
// 炼丹消耗
// 后羿
// 每种五行各一张
//
// 从牌山中移除：
// 忘忧堂
// 斩断尘缘，从牌山中移除
//
// 提升境界：
// 提升境界
// 百草堂，境界提升
//
// 复制：
// 天机阁，复制一次
//
// 属相相生：
// 天界树，属相相生
//
// 复杂：
// 卖牌
// 镜灵
// 分子打印机

using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Require Cell", -9, true)]
public class RequireCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Title = new("提交");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DetailedText = new("请提交卡");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<RequireCellBehaviorType> BehaviorType = new(RequireCellBehaviorType.Consume);
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<RunSkillDescriptorListModel> DescriptorList = new();
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> Success = new(self => self as ILogicNode);
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> Failure = new(self => self as ILogicNode);

    private bool _isSuccess = false;
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => _isSuccess ? Success : Failure;

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
        var title = Title.Value;
        var detailedText = DetailedText.Value;
        var behaviorType = BehaviorType.Value;
        var descriptorList = DescriptorList.Value;
        
        var requireCell = RequireCell.FromConstantDetailedText(
            titleText: title,
            detailedText: detailedText,
            descriptor: descriptorList
        );
        
        // 设置提交操作
        requireCell.SetSubmitOperation(cardPickerCell =>
        {
            bool fulfilled = cardPickerCell.AllFulfilled();
            
            // 根据行为类型处理
            switch (behaviorType)
            {
                case RequireCellBehaviorType.Consume:
                    if (!fulfilled)
                    {
                        cardPickerCell.WithdrawAll();
                        return GetFailureCell();
                    }
                    else
                    {
                        cardPickerCell.WithdrawAll();
                        return GetSuccessCell();
                    }
                    
                case RequireCellBehaviorType.RemoveFromPool:
                case RequireCellBehaviorType.UpgradeJingJieToCurrent:
                case RequireCellBehaviorType.UpgradeJingJieToNext:
                case RequireCellBehaviorType.Copy:
                case RequireCellBehaviorType.WuXingCycle:
                    // 这些类型只有成功分支
                    cardPickerCell.WithdrawAll();
                    return GetSuccessCell();
                    
                // case RequireCellBehaviorType.Complex:
                //     // 复杂情况暂时返回成功，后续可以扩展
                //     cardPickerCell.WithdrawAll();
                //     return GetSuccessCell();
                    
                default:
                    cardPickerCell.WithdrawAll();
                    return GetSuccessCell();
            }
        });
        
        return requireCell;
    }
    
    private Cell GetSuccessCell()
    {
        // var successNode = Success?.Connection?.Node as ILogicNode;
        // return successNode?.AsCell();
        return null;
    }
    
    private Cell GetFailureCell()
    {
        // var failureNode = Failure?.Connection?.Node as ILogicNode;
        // return failureNode?.AsCell();
        return null;
    }
    
    public override void ReceiveSignal(Signal signal)
    {
        // RequireCell会处理ConfirmDeckSignal，这里不需要额外处理
    }
}
