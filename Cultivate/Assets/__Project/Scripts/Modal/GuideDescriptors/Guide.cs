
public abstract class Guide
{
    private string _comment;
    public string GetComment() => _comment;

    public Guide(string comment)
    {
        _comment = comment;
    }

    public virtual void ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    { }

    public void SetComplete(PanelDescriptor panelDescriptor)
    {
        RunManager.Instance.Environment.GuideFinishNeuron.Invoke(this);
        panelDescriptor.MoveNextGuideDescriptor();
    }
}
