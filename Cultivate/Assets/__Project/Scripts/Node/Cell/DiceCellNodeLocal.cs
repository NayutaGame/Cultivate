
using System.Collections.Generic;
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;
using UnityEngine.Assertions;

[NodeWidth(400)]
[CreateNodeMenu("Cell/Dice Cell Local", -9, true)]
public class DiceCellNodeLocal : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<string> DiceDescription = new("你现在的想法是......");
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> DiceRange = new(20);

    [SerializeReference] private List<GainRow> GainTable;
    [SerializeReference] private List<ResultRow> ResultTable;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode>[] Paths;

    private int pathIndex = -1;
    
    protected override void Initialize()
    {
    }
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort 
    {
        get
        {
            if (pathIndex == -1)
                return null;

            return Paths[pathIndex];
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
        // 收集 GainRow 数组
        List<GainRow> gainRows = new List<GainRow>();
        if (GainTable != null)
        {
            foreach (var gainRow in GainTable)
            {
                gainRows.Add(gainRow);
            }
        }
        
        // 收集 ResultRow 数组
        List<ResultRow> resultRows = new List<ResultRow>();
        if (ResultTable != null)
        {
            foreach (var resultRow in ResultTable)
            {
                resultRows.Add(resultRow);
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
            Assert.IsTrue(0 <= pathIndex && pathIndex < Paths.Length, "ResultIndex is out of range");
            return true;
        }

        return false;
    }
}
