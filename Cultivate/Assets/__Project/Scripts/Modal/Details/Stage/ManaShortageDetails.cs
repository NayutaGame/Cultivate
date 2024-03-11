
public class ManaShortageDetails : EventDetails
{
    public StageEntity Owner;
    public int Position;
    public StageSkill Skill;
    public int Cost;

    public ManaShortageDetails(StageEntity owner, int position, StageSkill skill, int cost)
    {
        Owner = owner;
        Position = position;
        Skill = skill;
        Cost = cost;
    }
}
