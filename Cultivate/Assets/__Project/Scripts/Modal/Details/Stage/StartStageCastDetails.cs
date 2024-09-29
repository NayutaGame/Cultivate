
public class StartStageCastDetails : EventDetails
{
    public StageEnvironment Env;
    public StageEntity Caster;
    public StageSkill Skill;
    public bool Recursive;
    public CastResult CastResult;
    public int Times;

    public StartStageCastDetails(StageEnvironment env, StageEntity caster, StageSkill skill, bool recursive, CastResult castResult, int times = 1)
    {
        Env = env;
        Caster = caster;
        Skill = skill;
        Recursive = recursive;
        CastResult = castResult;
        Times = times;
    }
}
