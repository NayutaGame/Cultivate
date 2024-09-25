
public class RunResultPanelDescriptor : PanelDescriptor
{
    public RunResult RunResult;

    public RunResultPanelDescriptor(RunResult runResult)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
        };

        RunResult = runResult;
    }

    public override void DefaultEnter(PanelDescriptor panelDescriptor)
    {
        base.DefaultEnter(panelDescriptor);

        ProfileManager.WriteRunResultToCurrent(RunResult);
    }

    public override PanelDescriptor DefaultReceiveSignal(Signal signal)
    {
        if (signal is ReturnToTitleSignal returnToTitleSignal)
        {
            RunManager.Instance.ReturnToTitle();
            return this;
        }

        return this;
    }
}
