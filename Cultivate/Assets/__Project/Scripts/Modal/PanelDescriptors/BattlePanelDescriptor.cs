
using System;

public class BattlePanelDescriptor : PanelDescriptor
{
    private RunEntity _template;

    private RunEntity _enemy;
    public RunEntity GetEnemy() => _enemy;

    public void SetEnemy(RunEntity enemy)
    {
        _enemy = enemy;
        RunManager.Instance.Environment.SetAway(_enemy);
        RunManager.Instance.Environment.FieldChangedNeuron.Invoke();
    }

    public BattlePanelDescriptor(RunEntity template)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Enemy",                    GetEnemy },
        };
        _template = template;
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);
        SetEnemy(RunEntity.FromTemplate(_template));
    }

    public override void DefaultExit(PanelDescriptor panelDescriptor)
    {
        base.DefaultExit(panelDescriptor);
        SetEnemy(null);
    }

    private Func<PanelDescriptor> _winOperation;
    public BattlePanelDescriptor SetWinOperation(Func<PanelDescriptor> win)
    {
        _winOperation = win;
        return this;
    }

    private Func<PanelDescriptor> _loseOperation;
    public BattlePanelDescriptor SetLoseOperation(Func<PanelDescriptor> lose)
    {
        _loseOperation = lose;
        return this;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is BattleResultSignal battleResultSignal)
        {
            return (battleResultSignal.Win ? _winOperation : _loseOperation).Invoke();
        }

        if (signal is SkipCombatSignal skipCombatSignal)
        {
            return (skipCombatSignal.Win ? _winOperation : _loseOperation).Invoke();
        }

        return this;
    }

    public void Combat()
    {
        RunManager.Instance.Environment.Combat();
    }
}
