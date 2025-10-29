
public class GachaDetails : RunClosureDetails
{
    public SkillReference Skill;
    public int GachaIndex;

    public DeckIndex DeckIndex;
    
    public GachaDetails(SkillReference skill, int gachaIndex)
    {
        Skill = skill;
        GachaIndex = gachaIndex;
    }
}
