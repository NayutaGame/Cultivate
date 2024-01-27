
using System;
using System.Collections.Generic;
using CLLibrary;

public class Arena : ListModel<RunEntity>, Addressable
{
    private static readonly int ArenaSize = 6;

    private StageResult[] _reports;
    public StageResult[] Reports => _reports;

    public StageResult Result;

    private Dictionary<string, Func<object>> _accessors;
    public object Get(string s)
        => _accessors[s]();

    public Arena()
    {
        _accessors = new()
        {
            { "Briefs", () => _reports },
        };

        ArenaSize.Do(item =>
        {
            Add(RunEntity.Default());
        });

        _reports = new StageResult[ArenaSize * ArenaSize];
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
        {
            StageConfig d = new StageConfig(false, false, true, false, this[y], this[x], null);
            StageEnvironment environment = StageEnvironment.FromConfig(d);
            environment.Execute();
            _reports[y * ArenaSize + x] = environment.Result;
        }
    }

    public void ShowReport(int i)
    {
        Result = Reports[i];
    }
}
