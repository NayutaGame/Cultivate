
public class AddSkillDetails : ClosureDetails
{
    public DeckIndex DeckIndex;
    public RunSkill RunSkill;
    
    public AddSkillDetails(DeckIndex deckIndex, RunSkill runSkill)
    {
        DeckIndex = deckIndex;
        RunSkill = runSkill;
    }
}
