
public class UnequipGuide : Guide
{
    private SkillEntryDescriptor _from;

    public UnequipGuide(string comment, SkillEntryDescriptor from) : base(comment)
    {
        _from = from;
    }

    public override void ReceiveSignal(Cell cell, Signal signal)
    {
        if (signal is DeckChangedSignal && CheckComplete(out _))
            SetComplete(cell);
    }

    public bool CheckComplete(out DeckIndex from)
        => !RunManager.Instance.Environment.DeckIndexFromDescriptor(out from, _from, excludingHand: true);
}
