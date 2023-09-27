
using System;

public class BattlePanelDescriptor : PanelDescriptor
{
    private RunEntity _template;

    private RunEntity _enemy;
    public RunEntity Enemy
    {
        get => _enemy;
        set
        {
            if (_enemy != null) _enemy.EnvironmentChangedEvent -= RunManager.Instance.Battle.EnvironmentChanged;
            _enemy = value;
            if (_enemy != null) _enemy.EnvironmentChangedEvent += RunManager.Instance.Battle.EnvironmentChanged;
            _enemy?.EnvironmentChanged();
        }
    }

    public StageEnvironmentResult Result;
    public StageEnvironmentDetails StageEnvironmentDetails;

    public BattlePanelDescriptor(RunEntity template)
    {
        _accessors = new()
        {
            { "Enemy", () => Enemy },
        };
        _template = template;
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();
        Enemy = RunEntity.FromTemplate(_template);
        RunManager.Instance.Battle.EnvironmentChangedEvent -= RunManager.Instance.Battle.Simulate;
        RunManager.Instance.Battle.EnvironmentChangedEvent += Simulate;
        Simulate();
    }

    public override void DefaultExit()
    {
        base.DefaultExit();
        RunManager.Instance.Battle.EnvironmentChangedEvent -= Simulate;
        RunManager.Instance.Battle.EnvironmentChangedEvent += RunManager.Instance.Battle.Simulate;
        Enemy = null;
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

    public void Combat(bool animated, bool writeResult)
    {
        StageEnvironmentDetails d = new StageEnvironmentDetails(animated, writeResult, false, false, RunManager.Instance.Battle.Hero, Enemy);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
        Result = environment.Result;
    }

    private void Simulate()
    {
        StageEnvironmentDetails d = new StageEnvironmentDetails(false, false, false, false, RunManager.Instance.Battle.Hero, Enemy);
        StageEnvironment environment = new StageEnvironment(d);
        environment.Execute();
        Result = environment.Result;
    }
}
