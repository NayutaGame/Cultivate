
using System;
using System.Collections.Generic;
using CLLibrary;

public class BattleCell : Cell
{
    private RunEntity _template;
    private Func<Cell> _winOperation;
    private Func<Cell> _loseOperation;

    private RunEntity _enemy;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Guide",                      thisObject => ((BattleCell)thisObject).GetGuideDescriptor() },
        { "Enemy",                      thisObject => ((BattleCell)thisObject)._enemy },
    };
    public override object Get(string s) => Accessor[s](this);
    private BattleCell(RunEntity template)
    {
        _template = RunEntity.FromTemplate(template);
    }

    public static BattleCell FromTemplate(RunEntity template)
        => new(template);

    public static BattleCell FromName(string name)
        => FromQuery(EntityQuery.FromName(name));

    public static BattleCell FromQuery(EntityQuery query)
    {
        RunEntity template;
        
        if (query.LimitToPool)
        {
            if (!RunManager.Instance.Environment.EntityPool.TryDrawEntity(out template, query))
            {
                template = RunEntity.Default();
            }
        }
        else
        {
            template = EditorManager.FindEntity(query) ?? RunEntity.Default();
        }
        
        return new(template);
    }

    private void SetEnemy(RunEntity enemy)
    {
        _enemy = enemy;
        RunManager.Instance.Environment.SetAway(_enemy);
        RunManager.Instance.Environment.ResimulateNeuron.Invoke();
    }

    public BattleCell SetWinOperation(Func<Cell> win)
    {
        _winOperation = win;
        return this;
    }

    public BattleCell SetLoseOperation(Func<Cell> lose)
    {
        _loseOperation = lose;
        return this;
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
