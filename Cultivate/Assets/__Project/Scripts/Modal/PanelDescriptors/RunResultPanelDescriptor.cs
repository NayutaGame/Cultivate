
public class RunResultPanelDescriptor : PanelDescriptor
{
    public RunCommitDetails RunCommitDetails;

    public RunResultPanelDescriptor(RunCommitDetails runCommitDetails)
    {
        RunCommitDetails = runCommitDetails;
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
