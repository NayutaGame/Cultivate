
public class GachaDetails : RunClosureDetails
{
    public SkillGhost Skill;
    public int GachaIndex;

    public DeckIndex DeckIndex;
    
    public GachaDetails(SkillGhost skill, int gachaIndex)
    {
        Skill = skill;
        GachaIndex = gachaIndex;
    }
}
