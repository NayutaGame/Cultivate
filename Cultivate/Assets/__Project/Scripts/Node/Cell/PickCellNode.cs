
using System;
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;
using CLLibrary;

[NodeWidth(300)]
[CreateNodeMenu("Cell/Pick Cell", -9, true)]
public class PickCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Title = new(new("选牌"));

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DetailedText = new(new("请选择卡"));
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<PickCellBehaviorType> BehaviorType = new(PickCellBehaviorType.获得);

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<Bound> PickCardCountRange = new(new(1, 1));

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<EditorSkillEntryQuery>> DrawStrategies;

    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> ConfirmNext = new(self => self as ILogicNode);
    
    protected override void Initialize()
    {
    }
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => ConfirmNext;
    
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
        string detailedText = DetailedText.Value;
        Bound pickCardCountRange = PickCardCountRange.Value;
        
        List<SkillEntryQuery> drawStrategies = null;
        List<EditorSkillEntryQuery> editorQueries = DrawStrategies.Value;
        if (editorQueries != null && editorQueries.Count > 0)
        {
            drawStrategies = new List<SkillEntryQuery>();
            foreach (var editorQuery in editorQueries)
            {
                if (editorQuery != null)
                {
                    drawStrategies.Add(SkillEntryQuery.FromEditorQuery(editorQuery));
                }
            }
        }
        
        return new PickCell(
            titleText: title,
            detailedText: detailedText,
            pickCardCountRange: pickCardCountRange,
            drawStrategies: drawStrategies
        );
    }

    public override bool ReceiveSignal(Signal signal)
    {
        // (AsCell() as PickCell)?.DefaultConfirmOperation(selectedSkillsSignal);
        
        ConfirmSkillsSignal confirmSkillsSignal = signal as ConfirmSkillsSignal;
        if (confirmSkillsSignal == null)
            return false;
        
        // int ladder = Ladder.Value;
        // JingJie currJingJie = RoomDefinition.GetJingJieFromLadder(ladder);
        // JingJie nextJingJie = Mathf.Clamp(currJingJie + 1, 0, 4);
        PickCell pickCell = AsCell() as PickCell;
        
        List<SkillGhost> pickedSkills = pickCell.GetPickedSkillsFromPickedIndices(confirmSkillsSignal.PickedIndices);
        List<SkillGhost> abandonedSkills = pickCell.GetAbandonedSkillsFromPickedIndices(confirmSkillsSignal.PickedIndices);

        Dictionary<PickCellBehaviorType, Action<PickCell, List<SkillGhost>, List<SkillGhost>>> behaviorHandlers = new()
        {
            { PickCellBehaviorType.获得, (cell, picked, abandoned) =>
            {
                if (picked.Count > 0)
                    RunManager.Instance.Environment.PickSkillsProcedure(picked);
            }},
            { PickCellBehaviorType.获得和封印剩下, (cell, picked, abandoned) =>
            {
                if (picked.Count > 0)
                    RunManager.Instance.Environment.PickSkillsProcedure(picked);

                if (abandoned.Count > 0)
                    RunManager.Instance.Environment.SealProcedure(abandoned);
            }},
        };
        
        behaviorHandlers[BehaviorType.Value](pickCell, pickedSkills, abandonedSkills);
        return true;
    }
}