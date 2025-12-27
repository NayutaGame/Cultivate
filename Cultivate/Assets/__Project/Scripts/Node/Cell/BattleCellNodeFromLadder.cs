
using PuppyDragon.uNody;
using PuppyDragon.uNody.Logic;
using UnityEngine;
using System.Collections.Generic;

[NodeWidth(200)]
[CreateNodeMenu("Cell/Battle Cell From Ladder", -10, true)]
public class BattleCellNodeFromLadder : CellNode
{
    [ArrowPort, PortSettings(true, ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.Inherited)] [SerializeField]
    private InputPort<ILogicNode> prevs;
    
    [PortSettings(ShowBackingValue.Unconnected, ConnectionType.Override, TypeConstraint.None)] [SerializeField]
    private InputPort<int> Ladder;
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> WinNext = new(self => self as ILogicNode);
    
    [ArrowPort, PortSettings(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.Inherited)] [SerializeField]
    private OutputPort<ILogicNode> LoseNext = new(self => self as ILogicNode);
    
    private bool _isWin = false;
    
    public override NodePort PrevPort => prevs;
    public override NodePort NextPort => _isWin ? WinNext : LoseNext;

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
        EntityQuery entityQuery = EntityQuery.FromLadder(Ladder.Value);
        BattleCell battleCell = BattleCell.FromQuery(entityQuery);
        
        return battleCell;
    }
    
    public override bool ReceiveSignal(Signal signal)
    {
        if (signal is BattleResultSignal battleResultSignal)
        {
            _isWin = battleResultSignal.Win;
            RunManager.Instance.Environment.CommitBattleNeuron.Invoke(battleResultSignal.Win);
            return true;
        }
        else if (signal is SkipCombatSignal skipCombatSignal)
        {
            _isWin = skipCombatSignal.Win;
            RunManager.Instance.Environment.CommitBattleNeuron.Invoke(skipCombatSignal.Win);
            return true;
        }

        return false;
    }
}