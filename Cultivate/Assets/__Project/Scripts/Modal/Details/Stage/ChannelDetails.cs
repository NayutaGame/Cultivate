
public class ChannelDetails : EventDetails
{
    public StageEntity _caster;
    public StageSkill _skill;

    private int _channelTime;
    public int GetChannelTime() => _channelTime;

    private int _counter;
    public int GetCounter() => _counter;

    public ChannelDetails(StageEntity caster, StageSkill skill)
    {
        _caster = caster;
        _skill = skill;
        _channelTime = _skill.GetChannelTime();
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

    public ChannelDetails Clone()
        => new ChannelDetails(_caster, _skill) { _channelTime = _channelTime, _counter = _counter };
}
