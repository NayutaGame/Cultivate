
public class EquipGuide : Guide
{
    private SkillEntryDescriptor _from;
    private DeckIndex _to;

    public EquipGuide(string comment, SkillEntryDescriptor from, DeckIndex to) : base(comment)
    {
        _from = from;
        _to = to;
    }

    public override void ReceiveSignal(PanelDescriptor panelDescriptor, Signal signal)
    {
        if (signal is DeckChangedSignal fieldChangedSignal && CheckComplete(fieldChangedSignal))
            SetComplete(panelDescriptor);
    }

    public bool GetFlowOfIndices(out DeckIndex[] result)
    {
        result = new DeckIndex[2];
        result[1] = _to;
        return RunManager.Instance.Environment.FindDeckIndex(out result[0], _from, omit: new[] { _to });
    }

    public bool CheckComplete(DeckChangedSignal deckChangedSignal)
    {
        RunSkill skill = RunManager.Instance.Environment.GetSkillAtDeckIndex(_to);
        if (skill == null)
            return true;
        return _from.Contains(skill);
    }

    private void SetComplete(PanelDescriptor panelDescriptor)
    {
        panelDescriptor.MoveNextGuideDescriptor();
    }
}
