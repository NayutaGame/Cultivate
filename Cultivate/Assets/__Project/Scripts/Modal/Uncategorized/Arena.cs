using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Arena : ListModel<RunEntity>, Addressable
{
    private static readonly int ArenaSize = 6;

    private StageReport[] _reports;
    public StageReport[] Reports => _reports;

    public StageReport Report;

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
            Add(RunEntity.Default);
        });

        _reports = new StageReport[ArenaSize * ArenaSize];
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
            _reports[y * ArenaSize + x] = StageManager.SimulateBrief(this[y], this[x]);
        }
    }

    public void ShowReport(int i)
    {
        Report = Reports[i];
    }
}
