
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
        if (!Application.isPlaying)
            return null;
            
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

    public override void ReceiveSignal(Signal signal)
    {
        if (signal is ConfirmSkillsSignal selectedSkillsSignal)
        {
            (AsCell() as PickCell)?.DefaultReceiveSignal(selectedSkillsSignal);
        }
    }
}