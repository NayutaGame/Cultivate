
public class ChannelDetails : EventDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public int CurrCounter;
    public int MaxCounter;
    
    public ChannelDetails(StageEntity caster, StageSkill skill, int currCounter, int maxCounter)
    {
        Caster = caster;
        Skill = skill;
        CurrCounter = currCounter;
        MaxCounter = maxCounter;
    }
}
