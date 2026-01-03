
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;
using UnityEngine.Assertions;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Dice Cell", -10, true)]
public class DiceCellNode : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> PrevCell;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DiceDescription = new("你现在的想法是......");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> DiceRange = new(20);
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<GainRow>[] GainTable;
    
    [PortSettings(false, ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.Strict)] [SerializeField]
    private InputPort<ResultRow>[] ResultTable;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode>[] Paths;

    [PortSettings(false, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<int> FinalIndex;

    [PortSettings(false, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Strict)] [SerializeField]
    private OutputPort<int> FinalScore;

    private int pathIndex = -1;
    
    protected override void Initialize()
    {
    }
    
    public override NodePort PrevPort => PrevCell;
    public override NodePort NextPort 
    {
        get
        {
            if (pathIndex == -1)
                return null;

            return Paths[pathIndex];
        }
    }
    
    public override IEnumerable<ILogicNode> Prevs => PrevCell.Values;
    
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
        // 收集 GainRow 数组
        List<GainRow> gainRows = new List<GainRow>();
        if (GainTable != null)
        {
            foreach (var port in GainTable)
            {
                if (port != null && port.Value != null)
                {
                    gainRows.Add(port.Value);
                }
            }
        }
        
        // 收集 ResultRow 数组
        List<ResultRow> resultRows = new List<ResultRow>();
        if (ResultTable != null)
        {
            foreach (var port in ResultTable)
            {
                if (port != null && port.Value != null)
                {
                    resultRows.Add(port.Value);
                }
            }
        }
        
        return new DiceCell(
            diceRange: DiceRange.Value,
            gainTable: gainRows.ToArray(),
            resultTable: resultRows.ToArray(),
            diceDescription: DiceDescription.Value
        );
    }

    public override bool ReceiveSignal(Signal signal)
    {
        DiceCell diceCell = AsCell() as DiceCell;
        if (signal is RollSignal rollSignal)
        {
            diceCell.Roll();
            return false;
        }

        if (signal is ExitDiceSignal exitDiceSignal)
        {
            pathIndex = diceCell.GetResultIndex();
            FinalIndex.Value = diceCell.GetResultIndex();
            FinalScore.Value = diceCell.GetFinalDiceValue();
            
            Assert.IsTrue(0 <= pathIndex && pathIndex < Paths.Length, "ResultIndex is out of range");
            return true;
        }

        return false;
    }
}
