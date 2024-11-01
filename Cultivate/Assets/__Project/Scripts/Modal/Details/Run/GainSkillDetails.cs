
public class GainSkillDetails : ClosureDetails
{
    public DeckIndex DeckIndex;
    public RunSkill RunSkill;
    
    public GainSkillDetails(DeckIndex deckIndex, RunSkill runSkill)
    {
        DeckIndex = deckIndex;
        RunSkill = runSkill;
    }
}
