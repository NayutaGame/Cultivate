
public class EndStepDetails : StageClosureDetails
{
    public StageEntity Owner;
    public int P;
    public StageSkill Skill;

    public EndStepDetails(StageEnvironment env, StageEntity owner, int p) : base(env)
    {
        Owner = owner;
        P = p;
        
        Skill = owner.Skills[P];
    }
}
