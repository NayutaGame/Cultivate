
public class SetSkillDetails : RunClosureDetails
{
    public RunSkill Template;
    public DeckIndex DeckIndex;
    
    public SetSkillDetails(RunSkill template, DeckIndex deckIndex)
    {
        Template = template;
        DeckIndex = deckIndex;
    }
}
