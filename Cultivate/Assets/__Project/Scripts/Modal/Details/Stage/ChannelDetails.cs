
public class ChannelDetails : StageClosureDetails
{
    public StageEntity Caster;
    public StageSkill Skill;
    public int CurrCounter;
    public int MaxCounter;
    public int ProgressGain;
    
    public ChannelDetails(StageEntity caster, StageSkill skill, int currCounter, int maxCounter, int progressGain = 1)
    {
        Caster = caster;
        Skill = skill;
        CurrCounter = currCounter;
        MaxCounter = maxCounter;
        ProgressGain = progressGain;
    }
}
