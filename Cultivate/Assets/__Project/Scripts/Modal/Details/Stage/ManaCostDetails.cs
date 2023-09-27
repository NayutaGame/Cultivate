
public class ManaCostDetails : EventDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public int LiteralCost;
    public int ActualCost;

    public ManaCostDetails(StageEntity caster, StageSkill skill, int actualCost)
    {
        Caster = caster;
        Skill = skill;
        LiteralCost = skill.GetLiteralCost();
        ActualCost = actualCost;
    }
}
