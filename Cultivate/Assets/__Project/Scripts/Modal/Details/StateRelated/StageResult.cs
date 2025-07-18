
using System;
using System.Collections.Generic;
using System.Text;

public class StageResult : Addressable
{
    private StringBuilder _reportBuilder;
    private StageTimeline _timeline;
    public StageTimeline Timeline => _timeline;

    public bool WillEffectResult;

    public int HomeLeftHp;
    public int AwayLeftHp;

    // 0 正在打， 1 主场胜利， 2 客场胜利
    public int Flag;

    private static readonly Dictionary<string, Func<object, object>> Accessor = new()
    {
        { "Timeline",                   thisObject => ((StageResult)thisObject)._timeline },
    };
    public object Get(string s) => Accessor[s](this);
    public StageResult(StageConfig config)
    {
        if (config.GenerateReport)
            _reportBuilder = new StringBuilder();

        if (config.GenerateTimeline)
            _timeline = new StageTimeline();

        WillEffectResult = config.WillEffectResult;
    }

    // use Closure

    public void TryAppend(string s)
        => _reportBuilder?.Append(s);

    public void TryAppendNote(int entityIndex, StageSkill skill, CostDescription actualCostDescription, string actualDescription)
        => _timeline?.AppendNote(entityIndex, skill, actualCostDescription, actualDescription);

    public void TryAppendChannelNote(int entityIndex, StageSkill skill, int currCounter, int maxCounter)
        => _timeline?.AppendChannelNote(entityIndex, skill, currCounter, maxCounter);

    public override string ToString()
        => _reportBuilder?.ToString();
    
    public static StageResult FromConfig(StageConfig config)
    {
        StageEnvironment env = StageEnvironment.FromConfig(config);
        env.CoreProcedure().GetAwaiter().GetResult();
        return env.Result;
    }
}
