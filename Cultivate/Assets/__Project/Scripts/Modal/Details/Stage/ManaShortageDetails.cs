
public class ManaShortageDetails : EventDetails
{
    public StageEntity Owner;
    public int Position;
    public StageSkill Skill;
    public int LiteralCost;
    public int ActualCost;

    public ManaShortageDetails(StageEntity owner, int position, StageSkill skill, int actualCost)
    {
        Owner = owner;
        Position = position;
        Skill = skill;
        LiteralCost = Skill.GetLiteralCost();
        ActualCost = actualCost;
    }
}
