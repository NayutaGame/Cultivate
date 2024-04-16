
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
        RunManager.Instance.Environment.EnvironmentChanged();
    }

    public StageResult GetResult() => RunManager.Instance.Environment.SimulateResult;

    public BattlePanelDescriptor(RunEntity template)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Enemy",                    GetEnemy },
        };
        _template = template;
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();
        SetEnemy(RunEntity.FromTemplate(_template));
    }

    public override void DefaultExit()
    {
        base.DefaultExit();
        SetEnemy(null);
    }

    private Func<PanelDescriptor> _win;
    public BattlePanelDescriptor SetWin(Func<PanelDescriptor> win)
    {
        _win = win;
        return this;
    }

    private Func<PanelDescriptor> _lose;
    public BattlePanelDescriptor SetLose(Func<PanelDescriptor> lose)
    {
        _lose = lose;
        return this;
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is BattleResultSignal battleResultSignal)
        {
            return (battleResultSignal.State == BattleResultSignal.BattleResultState.Win ? _win : _lose).Invoke();
        }

        return this;
    }

    public void Combat()
    {
        RunManager.Instance.Environment.Combat();
    }
}
