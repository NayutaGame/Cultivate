
public class PuzzlePanelDescriptor : PanelDescriptor
{
    // private RunEntity _template;
    //
    // private RunEntity _enemy;
    // public RunEntity GetEnemy() => _enemy;

    // public void SetEnemy(RunEntity enemy)
    // {
    //     _enemy = enemy;
    //     RunManager.Instance.Environment.SetAway(_enemy);
    //     RunManager.Instance.Environment.EnvironmentChanged();
    // }
    //
    // public StageResult GetResult() => RunManager.Instance.Environment.SimulateResult;

    public PuzzlePanelDescriptor()
    {
        _accessors = new()
        {
            // { "Guide",                    GetGuideDescriptor },
            // { "Enemy",                    GetEnemy },
        };
    }

    // public override void DefaultEnter()
    // {
    //     base.DefaultEnter();
    //     SetEnemy(RunEntity.FromTemplate(_template));
    // }
    //
    // public override void DefaultExit()
    // {
    //     base.DefaultExit();
    //     SetEnemy(null);
    // }

    // private Func<PanelDescriptor> _winOperation;
    // public BattlePanelDescriptor SetWinOperation(Func<PanelDescriptor> win)
    // {
    //     _winOperation = win;
    //     return this;
    // }
    //
    // private Func<PanelDescriptor> _loseOperation;
    // public BattlePanelDescriptor SetLoseOperation(Func<PanelDescriptor> lose)
    // {
    //     _loseOperation = lose;
    //     return this;
    // }
    //
    // public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    // {
    //     if (signal is BattleResultSignal battleResultSignal)
    //     {
    //         return (battleResultSignal.State == BattleResultSignal.BattleResultState.Win ? _winOperation : _loseOperation).Invoke();
    //     }
    //
    //     return this;
    // }
    //
    // public void Combat()
    // {
    //     RunManager.Instance.Environment.Combat();
    // }
}
