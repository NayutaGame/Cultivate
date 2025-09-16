
public class SkillCostDefinition : RunCostDefinition
{
    private RunSkillDescriptor _descriptor;
    private string _description;

    public SkillCostDefinition(RunSkillDescriptor descriptor, string description)
    {
        _descriptor = descriptor;
        _description = description;
    }

    public override string GetDescription()
        => _description;

    public override bool Affordable()
    {
        return RunManager.Instance.Environment.DeckIndexFromDescriptor(out _, _descriptor);
    }

    public override bool Consume()
    {
        if (!Affordable())
            return false;

        RunManager.Instance.Environment.DeckIndexFromDescriptor(out DeckIndex deckIndex, _descriptor);
        RunManager.Instance.Environment.RemoveSkillProcedure(deckIndex);
        return true;
    }
}