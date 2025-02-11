
public class UnequipGuide : Guide
{
    private SkillEntryDescriptor _from;

    public UnequipGuide(string comment, SkillEntryDescriptor from) : base(comment)
    {
        _from = from;
    }

    public override bool ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    {
        if (signal is FieldChangedSignal && CheckComplete(out _))
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

    public bool CheckComplete(out DeckIndex from)
        => !RunManager.Instance.Environment.FindDeckIndex(out from, _from, excludingHand: true);
}
