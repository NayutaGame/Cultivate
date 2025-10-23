
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using PuppyDragon.uNody.Logic.BlackboardVariable;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Cell/Dialog Cell", -9, true)]
public class DialogCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Title;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DetailedText;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Option1String;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Option2String;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Option3String;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> Option4String;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> Option1 = new(self => self as ILogicNode);
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> Option2 = new(self => self as ILogicNode);
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> Option3 = new(self => self as ILogicNode);
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> Option4 = new(self => self as ILogicNode);
    
    protected override void Initialize()
    {
    }
    // 存储玩家选择的选项索引
    private int selectedOptionIndex = -1;
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort 
    { 
        get 
        {
            // 根据选择的选项索引返回对应的输出端口
            return selectedOptionIndex switch
            {
                0 => Option1,
                1 => Option2,
                2 => Option3,
                3 => Option4,
                _ => null
            };
        }
    }
    
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
        var options = new List<DialogOption>();
        
        var optionStrings = new[] { Option1String.Value, Option2String.Value, Option3String.Value, Option4String.Value };
        
        foreach (var optionString in optionStrings)
        {
            if (!string.IsNullOrEmpty(optionString))
                options.Add(DialogOption.FromText(optionString));
        }
        
        if (options.Count == 0)
            options.Add(DialogOption.FromText("继续"));
        
        return new DialogCell(
            Title.Value,
            DetailedText.Value,
            options.ToArray()
        );
    }

    public override void ReceiveSignal(Signal signal)
    {
        if (signal is SelectedOptionSignal selectedOptionSignal)
        {
            selectedOptionIndex = selectedOptionSignal.Selected;
        }
    }
}