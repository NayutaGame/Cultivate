
public class ExecuteDetails : ClosureDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public int CastTimes;

    public ExecuteDetails(StageEntity caster, StageSkill skill)
    {
        Caster = caster;
        Skill = skill;
        CastTimes = 1;
    }
}
