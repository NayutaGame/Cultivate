
using System;
using System.Collections.Generic;
using System.Text;

public class StageResult : Addressable
{
    private StringBuilder _reportBuilder;
    private StageTimeline _timeline;
    public StageTimeline Timeline => _timeline;

    public bool WriteResult;

    public int HomeLeftHp;
    public int AwayLeftHp;

    private bool _homeVictory;
    public bool HomeVictory => _homeVictory;
    public void SetHomeVictory(bool value) => _homeVictory = value;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s) => _accessors[s]();
    public StageResult(StageConfig config)
    {
        _accessors = new()
        {
            { "Timeline",              () => _timeline },
        };

        if (config.GenerateReport)
            _reportBuilder = new StringBuilder();

        if (config.GenerateTimeline)
            _timeline = new StageTimeline();

        WriteResult = config.WriteResult;
    }

    // use Neuron (observer pattern)

    public void TryAppend(string s)
        => _reportBuilder?.Append(s);

    public void TryAppendNote(int entityIndex, StageSkill skill)
        => _timeline?.AppendNote(entityIndex, skill);

    public void TryAppendChannelNote(int entityIndex, ChannelDetails d)
        => _timeline?.AppendChannelNote(entityIndex, d);

    public override string ToString()
        => _reportBuilder?.ToString();
}
