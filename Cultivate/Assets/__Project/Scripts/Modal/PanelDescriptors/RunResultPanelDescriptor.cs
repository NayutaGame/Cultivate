
public class RunResultPanelDescriptor : PanelDescriptor
{
    public RunResult RunResult;

    public RunResultPanelDescriptor(RunResult runResult)
    {
        RunResult = runResult;
    }

    public override void DefaultEnter()
    {
        base.DefaultEnter();

        // Write Unlocks to profile and save profile
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
