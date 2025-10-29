
public class UnequipGuide : Guide
{
    private RunSkillQuery _from;

    public UnequipGuide(string comment, RunSkillQuery from) : base(comment)
    {
        _from = from;
    }

    public override void ReceiveSignal(Cell cell, Signal signal)
    {
        if (signal is DeckChangedSignal && CheckComplete(out _))
            SetComplete(cell);
    }

    public bool CheckComplete(out DeckIndex from)
        => !RunManager.Instance.Environment.DeckIndexFromQuery(out from, _from, excludingHand: true);
}
