
public class ConfirmGuide : Guide
{
    public ConfirmGuide(string comment) : base(comment)
    {
    }

    public override void ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    {
        if (signal is ConfirmGuideSignal)
            SetComplete(panelDescriptor);
    }
    
    private void SetComplete(PanelDescriptor panelDescriptor)
    {
        panelDescriptor.MoveNextGuideDescriptor();
    }
}
