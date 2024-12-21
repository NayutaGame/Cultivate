
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

    public int Flag;

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

    // use Closure

    public void TryAppend(string s)
        => _reportBuilder?.Append(s);

    public void TryAppendNote(int entityIndex, StageSkill skill, CostResult costResult, CastResult castResult)
        => _timeline?.AppendNote(entityIndex, skill, costResult, castResult);

    public void TryAppendChannelNote(int entityIndex, StageSkill skill, int currCounter, int maxCounter)
        => _timeline?.AppendChannelNote(entityIndex, skill, currCounter, maxCounter);

    public override string ToString()
        => _reportBuilder?.ToString();
}
