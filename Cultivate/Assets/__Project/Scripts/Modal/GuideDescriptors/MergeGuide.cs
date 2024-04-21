
public class MergeGuide : Guide
{
    private string _comment;
    public string GetComment() => _comment;
    private SkillDescriptor _from;
    private SkillDescriptor _to;

    public MergeGuide(string comment, SkillDescriptor from, SkillDescriptor to)
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

    private void SetComplete(PanelDescriptor panelDescriptor)
    {
        panelDescriptor.MoveNextGuideDescriptor();
    }

    public bool CheckComplete(out DeckIndex[] result)
    {
        result = new DeckIndex[2];

        bool hasFrom = RunManager.Instance.Environment.FindDeckIndex(out result[0], _from, excludingField: true);
        if (!hasFrom)
            return true;

        return !RunManager.Instance.Environment.FindDeckIndex(out result[1], _to, excludingField: true,
            omit: new[] { result[0] });
    }
}
