
public class ConfirmGuide : Guide
{
    public ConfirmGuide(string comment) : base(comment)
    {
    }

    public override void ReceiveSignal(Cell cell, Signal signal)
    {
        if (signal is ConfirmGuideSignal)
            SetComplete(cell);
    }
}
