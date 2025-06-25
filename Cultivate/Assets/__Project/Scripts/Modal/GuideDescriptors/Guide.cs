
public abstract class Guide
{
    private string _comment;
    public string GetComment() => _comment;

    public Guide(string comment)
    {
        _comment = comment;
    }

    public virtual void ReceiveSignal(Cell cell, Signal signal)
    { }

    public void SetComplete(Cell cell)
    {
        RunManager.Instance.Environment.GuideFinishNeuron.Invoke(this);
        cell.MoveNextGuideDescriptor();
    }
}
