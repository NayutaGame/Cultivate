
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

    public StageReport Report;
    public CombatDetails CombatDetails;

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
        RunManager.Instance.Battle.EnvironmentChangedEvent += CalcReport;
        CalcReport();
    }

    public override void DefaultExit()
    {
        base.DefaultExit();
        RunManager.Instance.Battle.EnvironmentChangedEvent -= CalcReport;
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
        StageManager.Instance.CombatDetails = new CombatDetails(animated, writeResult, RunManager.Instance.Battle.Hero, Enemy, Report);
        AppManager.Push(new StageAppS());
    }

    private void CalcReport()
    {
        Report = StageManager.SimulateBrief(RunManager.Instance.Battle.Hero, Enemy);
    }
}
