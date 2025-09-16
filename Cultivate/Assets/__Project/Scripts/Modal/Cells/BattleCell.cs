
using System;
using System.Collections.Generic;

public class BattleCell : Cell
{
    private RunEntity _template;

    private RunEntity _enemy;
    public RunEntity GetEnemy() => _enemy;

    public void SetEnemy(RunEntity enemy)
    {
        _enemy = enemy;
        RunManager.Instance.Environment.SetAway(_enemy);
        RunManager.Instance.Environment.ResimulateNeuron.Invoke();
    }

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((BattleCell)thisObject).GetGuideDescriptor() },
        { "Enemy",                      thisObject => ((BattleCell)thisObject).GetEnemy() },
    };
    public override object Get(string s) => Accessor[s](this);
    public BattleCell(RunEntity template)
    {
        _template = template;
    }

    public override void DefaultEnter(Cell cell)
    {
        base.DefaultEnter(cell);
        SetEnemy(RunEntity.FromTemplate(_template));
    }

    public override void DefaultExit(Cell cell)
    {
        base.DefaultExit(cell);
        SetEnemy(null);
        
        RunManager.Instance.Environment.Home.ClearSlotResults();
    }

    private Func<Cell> _winOperation;
    public BattleCell SetWinOperation(Func<Cell> win)
    {
        _winOperation = win;
        return this;
    }

    private Func<Cell> _loseOperation;
    public BattleCell SetLoseOperation(Func<Cell> lose)
    {
        _loseOperation = lose;
        return this;
    }

    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is BattleResultSignal battleResultSignal)
        {
            RunManager.Instance.Environment.CommitBattleNeuron.Invoke(battleResultSignal.Win);
            return (battleResultSignal.Win ? _winOperation : _loseOperation)?.Invoke();
        }

        if (signal is SkipCombatSignal skipCombatSignal)
        {
            RunManager.Instance.Environment.CommitBattleNeuron.Invoke(skipCombatSignal.Win);
            return (skipCombatSignal.Win ? _winOperation : _loseOperation)?.Invoke();
        }

        return this;
    }
}
