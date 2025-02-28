
public class ExhaustDetails : StageClosureDetails
{
    public StageEntity Owner;
    public StageSkill Skill;

    public ExhaustDetails(StageEntity owner, StageSkill skill)
    {
        Owner = owner;
        Skill = skill;
    }
}
