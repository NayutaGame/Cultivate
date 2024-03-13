
public class CastDetails : EventDetails
{
    public StageEntity Caster;
    public StageSkill Skill;

    public CastDetails(StageEntity caster, StageSkill skill)
    {
        Caster = caster;
        Skill = skill;
    }
}
