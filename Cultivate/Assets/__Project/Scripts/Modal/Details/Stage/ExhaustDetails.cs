
public class ExhaustDetails : StageClosureDetails
{
    public StageEntity Owner;
    public StageSkill Skill;

    public ExhaustDetails(StageEnvironment env, StageEntity owner, StageSkill skill) : base(env)
    {
        Owner = owner;
        Skill = skill;
    }
}
