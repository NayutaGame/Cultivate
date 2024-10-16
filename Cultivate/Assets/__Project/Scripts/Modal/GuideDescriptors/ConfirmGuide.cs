
public class ConfirmGuide : Guide
{
    public ConfirmGuide(string comment) : base(comment)
    {
    }

    public override bool ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    {
        if (signal is ConfirmGuideSignal)
        {
            SetComplete(panelDescriptor);
            return true;
        }
    
        return false;
    }
    
    private void SetComplete(PanelDescriptor panelDescriptor)
    {
        panelDescriptor.MoveNextGuideDescriptor();
    }
}
