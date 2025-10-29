
public class MergeGuide : Guide
{
    private RunSkillQuery _from;
    private RunSkillQuery _to;

    public MergeGuide(string comment, RunSkillQuery from, RunSkillQuery to) : base(comment)
    {
        _from = from;
        _to = to;
    }

    public override void ReceiveSignal(Cell cell, Signal signal)
    {
        if (signal is DeckChangedSignal && CheckComplete(out _))
            SetComplete(cell);
    }

    public bool CheckComplete(out DeckIndex[] result)
    {
        result = new DeckIndex[2];

        bool hasFrom = RunManager.Instance.Environment.DeckIndexFromQuery(out result[0], _from, excludingField: true);
        if (!hasFrom)
            return true;

        return !RunManager.Instance.Environment.DeckIndexFromQuery(out result[1], _to, excludingField: true,
            omit: new[] { result[0] });
    }
}
