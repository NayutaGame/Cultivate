
using System;
using System.Collections.Generic;
using System.Text;

public class StageResult : Addressable
{
    private StringBuilder _reportBuilder;
    private StageTimeline _timeline;
    public StageTimeline Timeline => _timeline;

    public int HomeLeftHp;
    public int AwayLeftHp;

    private bool _homeVictory;
    public bool HomeVictory => _homeVictory;
    public void SetHomeVictory(bool value) => _homeVictory = value;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public StageResult(bool generateReport, bool generateTimeline)
    {
        _accessors = new()
        {
            { "Timeline",              () => _timeline },
        };

        if (generateReport)
            _reportBuilder = new StringBuilder();

        if (generateTimeline)
            _timeline = new StageTimeline();
    }

    // use Neuron

    public void TryAppend(string s)
        => _reportBuilder?.Append(s);

    public void TryAppendNote(int entityIndex, StageSkill skill)
        => _timeline?.AppendNote(entityIndex, skill);

    public void TryAppendChannelNote(int entityIndex, ChannelDetails d)
        => _timeline?.AppendChannelNote(entityIndex, d);

    public override string ToString()
        => _reportBuilder?.ToString();
}
