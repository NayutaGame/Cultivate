
public class StartStageCastDetails : EventDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public int Times;

    public StartStageCastDetails(StageEntity caster, StageSkill skill, int times = 1)
    {
        Caster = caster;
        Skill = skill;
        Times = times;
    }
}
