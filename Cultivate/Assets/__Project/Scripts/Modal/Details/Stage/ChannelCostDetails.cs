
public class ChannelCostDetails : EventDetails
{
    public StageEntity Caster;
    public StageSkill Skill;

    private int _channelTime;
    public int GetChannelTime() => _channelTime;

    private int _counter;
    public int GetCounter() => _counter;

    public ChannelCostDetails(StageEntity caster, StageSkill skill, int channelTime)
    {
        Caster = caster;
        Skill = skill;
        _channelTime = channelTime;
        
        _counter = _channelTime;
    }

    public void IncrementProgress()
    {
        _counter -= 1;
    }

    public bool FinishedChannelling()
    {
        return _counter <= 0;
    }

    public ChannelCostDetails Clone()
        => new(Caster, Skill, _channelTime) { _counter = _counter };
}
