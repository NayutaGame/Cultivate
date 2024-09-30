
public class EndStepDetails : ClosureDetails
{
    public StageEntity Owner;
    public int P;
    public StageSkill Skill;

    public EndStepDetails(StageEntity owner, int p)
    {
        Owner = owner;
        P = p;
        
        Skill = owner._skills[P];
    }
}
