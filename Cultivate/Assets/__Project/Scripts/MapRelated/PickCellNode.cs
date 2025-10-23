
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
    private InputPort<Bound> Bound = new(new Bound(0, 2));

    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<SkillEntryCollectionDescriptor> DrawStrategy = new(new SkillEntryCollectionDescriptor());

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
        var drawStrategy = DrawStrategy.Value;
        
        var pickCell = new PickCell(
            titleText: title,
            detailedText: detailedText,
            bound: bound
        );
        
        var inventory = GenerateInventoryFromDescriptor(drawStrategy);
        pickCell.PopulateInventory(inventory);
        
        return pickCell;
    }
    
    private List<SkillEntryDescriptor> GenerateInventoryFromDescriptor(SkillEntryCollectionDescriptor drawStrategy)
    {
        GainSkillBuilder b = new();
        b.Draw(drawStrategy);
        
        return b.DrawnSkillEntries
            .FilterObj(e => e != Encyclopedia.SkillCategory.Default())    
            .Map(e => SkillEntryDescriptor.FromEntryJingJie(e, RunManager.Instance.Environment.JingJie))
            .ToList();
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
                    b.Pick(item.Entry);
                    b.SingleCreate(item.JingJie);
                });
                b.Add();
                b.Invoke();
            }
        }
    }
}