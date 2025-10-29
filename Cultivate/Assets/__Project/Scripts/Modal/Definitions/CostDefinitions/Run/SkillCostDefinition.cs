
public class SkillCostDefinition : RunCostDefinition
{
    private RunSkillQuery _query;
    private string _description;

    public SkillCostDefinition(RunSkillQuery query, string description)
    {
        _query = query;
        _description = description;
    }

    public override string GetDescription()
        => _description;

    public override bool Affordable()
    {
        return RunManager.Instance.Environment.DeckIndexFromQuery(out _, _query);
    }

    public override bool Consume()
    {
        if (!Affordable())
            return false;

        RunManager.Instance.Environment.DeckIndexFromQuery(out DeckIndex deckIndex, _query);
        RunManager.Instance.Environment.RemoveSkillProcedure(deckIndex);
        return true;
    }
}