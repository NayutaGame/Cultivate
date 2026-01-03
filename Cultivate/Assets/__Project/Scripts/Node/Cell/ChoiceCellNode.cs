
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using PuppyDragon.uNody.Logic.BlackboardVariable;
using UnityEngine;

[NodeWidth(300)]
[CreateNodeMenu("Cell/Choice Cell", -10, true)]
public class ChoiceCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
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
    
    private int selectedOptionIndex = -1;
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort 
    {
        get 
        {
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
        var options = new List<ChoiceOption>();
        
        var optionStrings = new[] { Option1String.Value, Option2String.Value, Option3String.Value, Option4String.Value };

        for (int i = 0; i < optionStrings.Length; i++)
        {
            string optionString = optionStrings[i];
            if (!string.IsNullOrEmpty(optionString))
                options.Add(ChoiceOption.FromIndexText(i, optionString));
        }
        
        if (options.Count == 0)
            options.Add(ChoiceOption.FromIndex(0));
        
        return new ChoiceCell(options);
    }

    public override bool ReceiveSignal(Signal signal)
    {
        if (signal is SelectedChoiceSignal selectedChoiceSignal)
        {
            selectedOptionIndex = selectedChoiceSignal.Selected;
            return true;
        }
        
        return false;
    }
}