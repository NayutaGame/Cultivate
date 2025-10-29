
using System.Collections.Generic;
using System.Linq;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;
using CLLibrary;

[NodeWidth(300)]
[CreateNodeMenu("Cell/Pick Cell", -9, true)]
public class PickCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] 
    [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Title = new(new("选牌"));

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DetailedText = new(new("请选择卡"));

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<Bound> Bound = new(new(1, 1));

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<List<SkillEntryQuery>> DrawStrategies = new(null);

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
        var title = Title.Value;
        var detailedText = DetailedText.Value;
        var bound = Bound.Value;
        var drawStrategies = DrawStrategies.Value;
        
        var pickCell = new PickCell(
            titleText: title,
            detailedText: detailedText,
            bound: bound
        );
        
        GainSkillBuilder b = new();
        b.Draw(drawStrategies, RunManager.Instance.Environment.JingJie, distinct: true, consume: false);
        b.GainingSkills.Do(g => pickCell.PopulateInventory(SkillReference.FromGainingSkill(g)));
        
        return pickCell;
    }

    public override void ReceiveSignal(Signal signal)
    {
        if (signal is ConfirmSkillsSignal selectedSkillsSignal)
        {
            var selectedSkills = selectedSkillsSignal.Selected;
            
            if (selectedSkills.Count > 0)
            {
                GainSkillBuilder b = new();
                selectedSkills.Do(item =>
                {
                    b.Pick(item.Clone());
                });
                b.Execute();
                b.Invoke();
            }
        }
    }
}