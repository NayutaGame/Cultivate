
public class EquipGuide : Guide
{
    private string _comment;
    public string GetComment() => _comment;
    private SkillDescriptor _from;
    private DeckIndex _to;

    public EquipGuide(string comment, SkillDescriptor from, DeckIndex to)
    {
        _comment = comment;
        _from = from;
        _to = to;
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

    public bool CheckComplete(out DeckIndex[] result)
    {
        result = new DeckIndex[2];
        result[1] = _to;
        return !RunManager.Instance.Environment.FindDeckIndex(out result[0], _from, omit: new[] { _to });
    }

    private void SetComplete(PanelDescriptor panelDescriptor)
    {
        panelDescriptor.MoveNextGuideDescriptor();
    }
}
