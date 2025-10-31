
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

using System;
using System.Collections.Generic;
using CLLibrary;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;
using UnityEngine.Analytics;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Require Cell", -9, true)]
public class RequireCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Title = new("提交");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DetailedText = new();
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<RequireCellBehaviorType> BehaviorType = new(RequireCellBehaviorType.耗材);

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<EditorRunSkillQuery>> Requirements;
    
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
        if (!Application.isPlaying)
            return null;

        var title = Title.Value;
        var detailedText = DetailedText.Value;

        List<RunSkillQuery> runQueries = null;
        List<EditorRunSkillQuery> editorQueries = Requirements.Value;
        if (editorQueries != null && editorQueries.Count > 0)
        {
            runQueries = new List<RunSkillQuery>(editorQueries.Count);
            for (int i = 0; i < editorQueries.Count; i++)
            {
                var editorQuery = editorQueries[i];
                if (editorQuery == null) continue;
                runQueries.Add(RunSkillQuery.FromEditorQuery(editorQuery));
            }
        }

        var requireCell = RequireCell.FromConstantDetailedText(
            titleText: title,
            detailedText: detailedText,
            requirements: runQueries
        );

        return requireCell;
    }
    
    public override void ReceiveSignal(Signal signal)
    {
        ConfirmDeckSignal confirmDeckSignal = signal as ConfirmDeckSignal;
        if (confirmDeckSignal == null)
            return;

        int ladder = 8; // From blackboard
        JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        JingJie nextJingJie = Mathf.Clamp(currJingJie + 1, 0, 4);

        Dictionary<RequireCellBehaviorType, Func<RequireCell, bool>> behaviorHandlers = new()
        {
            { RequireCellBehaviorType.耗材, requireCell =>
            {
                bool success = requireCell.AllFulfilled();
                if (!success)
                {
                    requireCell.WithdrawAll();
                }

                return success;
            }},
            { RequireCellBehaviorType.移除, requireCell =>
            {
                bool success = requireCell.AnyFulfilled();
                
                requireCell.RequirementSlotList.Do(slot =>
                {
                    RunSkill skillToDepopulate = slot.Skill;
                    if (skillToDepopulate == null)
                        return;

                    RunManager.Instance.Environment.SkillPool.Depopulate(pred: e => e == skillToDepopulate.GetEntry());
                });

                for (int i = 0; i < requireCell.RequirementSlotList.Count(); i++)
                {
                    var slot = requireCell.RequirementSlotList[i];
                    
                    if (slot.Skill == null)
                        continue;
                
                    JingJie targetJingJie = slot.Skill.GetJingJie();
                
                    GainSkillBuilder b = new();
                    SkillEntryQuery drawStrategy = SkillEntryQuery.FromBaseJingJieBound(new(JingJie.LianQi, targetJingJie));
                    b.Draw(drawStrategy, targetJingJie);
                    slot.Skill = RunSkill.FromGainingSkill(b.GainingSkills[0]);
                }
                        
                requireCell.WithdrawAll();

                return success;
            }},
            { RequireCellBehaviorType.提升境界至当前境界, requireCell =>
            {
                bool success = requireCell.AnyFulfilled();
                
                requireCell.RequirementSlotList.Do(slot =>
                {
                    if (slot.Skill == null)
                        return;
                
                    slot.Skill = RunSkill.FromChangeJingJie(slot.Skill, currJingJie);
                });
                        
                requireCell.WithdrawAll();
                return success;
            }},
            { RequireCellBehaviorType.提升境界至下一境界, requireCell =>
            {
                bool success = requireCell.AnyFulfilled();
                
                requireCell.RequirementSlotList.Do(slot =>
                {
                    if (slot.Skill == null)
                        return;
                
                    slot.Skill = RunSkill.FromChangeJingJie(slot.Skill, nextJingJie);
                });
                        
                requireCell.WithdrawAll();
                return success;
            }},
            { RequireCellBehaviorType.复制, requireCell =>
            {
                bool success = requireCell.AnyFulfilled();
                if (!success)
                {
                    requireCell.WithdrawAll();
                    return false;
                }

                int count = requireCell.RequirementSlotList.Count();
                RequirementSlot copyingSlot = requireCell.RequirementSlotList[RandomManager.Range(0, count)];
                RunSkill copyingSkill = copyingSlot.Skill;
            
                RunManager.Instance.Environment.PickSkillProcedure(copyingSkill.GetEntry(), copyingSkill.GetJingJie());
                        
                requireCell.WithdrawAll();
                return true;
            }},
            { RequireCellBehaviorType.五行相生, requireCell =>
            {
                bool success = requireCell.AnyFulfilled();
                
                requireCell.RequirementSlotList.Do(slot =>
                {
                    if (slot.Skill == null)
                        return;

                    if (slot.Skill.GetWuXing() == WuXing.Wu)
                        return;

                    WuXing targetWuXing = slot.Skill.GetWuXing().Next;
                    JingJie targetJingJie = slot.Skill.GetJingJie();
                
                    GainSkillBuilder b = new();
                    SkillEntryQuery drawStrategy = SkillEntryQuery.FromWuXingBaseJingJieBound(
                        wuXing: targetWuXing,
                        baseJingJieBound: new(JingJie.LianQi, targetJingJie));
                    b.Draw(drawStrategy, jingJie: targetJingJie, consume: true);
                    slot.Skill = RunSkill.FromGainingSkill(b.GainingSkills[0]);
                });
                        
                requireCell.WithdrawAll();
                return success;
            }},
        };
        
        _isSuccess = behaviorHandlers[BehaviorType.Value](AsCell() as RequireCell);
    }
}
