
using System;
using System.Collections.Generic;
using CLLibrary;

public class Arena : ListModel<RunEntity>, Addressable
{
    private static readonly int ArenaSize = 6;

    private StageResult[] _results;
    public StageResult[] Results => _results;

    public StageResult Result;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public Arena()
    {
        _accessors = new()
        {
            { "Briefs", () => _results },
        };

        ArenaSize.Do(item =>
        {
            Add(RunEntity.Default());
        });

        _results = new StageResult[ArenaSize * ArenaSize];
    }

    public bool TryWrite(RunSkill fromSkill, SkillSlot toSlot)
    {
        toSlot.Skill = fromSkill;
        return true;
    }

    public bool TryWrite(SkillSlot fromSlot, SkillSlot toSlot)
    {
        toSlot.Skill = fromSlot.Skill;
        return true;
    }

    public bool TryIncreaseJingJie(SkillSlot slot)
        => slot.TryIncreaseJingJie();

    public void Compete()
    {
        for (int y = 0; y < ArenaSize; y++)
        for (int x = 0; x < ArenaSize; x++)
            _results[y * ArenaSize + x] = StageEnvironment.CalcSimulateResult(StageConfig.ForSimulate(this[y], this[x], null));
    }

    public void ShowReport(int i)
    {
        Result = Results[i];
    }
}
