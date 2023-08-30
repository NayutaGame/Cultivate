using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelDetails : EventDetails
{
    public StageSkill _skill;

    private int _channelTime;
    public int GetChannelTime() => _channelTime;

    private int _counter;
    public int GetCounter() => _counter;

    public ChannelDetails(StageSkill skill)
    {
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
}
