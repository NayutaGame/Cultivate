
public class GainSkillDetails : RunClosureDetails
{
    public DeckIndex DeckIndex;
    public RunSkill RunSkill;
    
    public GainSkillDetails(DeckIndex deckIndex, RunSkill runSkill)
    {
        DeckIndex = deckIndex;
        RunSkill = runSkill;
    }
}
