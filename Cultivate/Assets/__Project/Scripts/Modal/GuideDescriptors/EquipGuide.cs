
public class EquipGuide : Guide
{
    private SkillEntryDescriptor _from;
    private DeckIndex _to;

    public EquipGuide(string comment, SkillEntryDescriptor from, DeckIndex to) : base(comment)
    {
        _from = from;
        _to = to;
    }

    public override void ReceiveSignal(Cell cell, Signal signal)
    {
        if (signal is DeckChangedSignal deckChangedSignal && CheckComplete(deckChangedSignal))
            SetComplete(cell);
    }

    public bool GetFlowOfIndices(out DeckIndex[] result)
    {
        result = new DeckIndex[2];
        result[1] = _to;
        return RunManager.Instance.Environment.DeckIndexFromDescriptor(out result[0], _from, omit: new[] { _to });
    }

    public bool CheckComplete(DeckChangedSignal deckChangedSignal)
    {
        RunSkill skill = RunManager.Instance.Environment.SkillFromDeckIndex(_to);
        if (skill == null)
            return false;
        return _from.Contains(skill);
    }
}
