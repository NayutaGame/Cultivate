
public class ManaCostDetails : EventDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public int Cost;

    public ManaCostDetails(StageEntity caster, StageSkill skill, int cost)
    {
        Caster = caster;
        Skill = skill;
        Cost = cost;
    }
}
