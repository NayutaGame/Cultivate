
public class ReplaceSkillDetails : ClosureDetails
{
    public RunSkill Template;
    public DeckIndex DeckIndex;
    
    public ReplaceSkillDetails(RunSkill template, DeckIndex deckIndex)
    {
        Template = template;
        DeckIndex = deckIndex;
    }
}
