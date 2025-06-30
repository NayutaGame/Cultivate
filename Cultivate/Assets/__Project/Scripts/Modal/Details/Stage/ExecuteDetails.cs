
public class ExecuteDetails : StageClosureDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public int CastTimes;

    public ExecuteDetails(StageEnvironment env, StageEntity caster, StageSkill skill) : base(env)
    {
        Caster = caster;
        Skill = skill;
        CastTimes = 1;
    }
}
