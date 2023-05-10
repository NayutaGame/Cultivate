using System;
using System.Collections;
using System.Collections.Generic;
using CLLibrary;
using UnityEngine;

public class Arena : Inventory<RunEntity>, GDictionary
{
    private static readonly int ArenaSize = 6;

    private StageReport[] _reports;
    public StageReport[] Reports => _reports;

    public StageReport Report;

    public SkillInventory SkillInventory;

    protected DragDropDelegate _dragDropDelegate;

    private Dictionary<string, Func<object>> _accessors;
    public Dictionary<string, Func<object>> GetAccessors() => _accessors;
    public Arena()
    {
        _accessors = new()
        {
            { "Briefs", () => _reports },
            { "SkillInventory", () => SkillInventory },
        };

        _dragDropDelegate = new(2, new Func<IDragDrop, IDragDrop, bool>[]
            {
                /*               RunSkill,   SkillSlot */
                /* RunSkill   */ null,       TryWrite,
                /* SkillSlot  */ null,       TryWrite,
            },
            item =>
            {
                if (item is RunSkill)
                    return 0;
                if (item is SkillSlot)
                    return 1;

                return null;
            });

        SkillInventory = new();
        SkillInventory.SetDragDropDelegate(_dragDropDelegate);
        Encyclopedia.SkillCategory.Traversal.Map(e => new RunSkill(e, e.JingJieRange.Start)).Do(e => SkillInventory.AddSkill(e));

        ArenaSize.Do(item =>
        {
            RunEntity e = new RunEntity();
            e.SetDragDropDelegate(_dragDropDelegate);
            Add(e);
        });

        _reports = new StageReport[ArenaSize * ArenaSize];
    }

    private bool TryWrite(IDragDrop from, IDragDrop to)
    {
        RunSkill skill = null;
        if (from is RunSkill fromSkill)
        {
            skill = fromSkill;
        }
        else if (from is SkillSlot skillSlot)
        {
            skill = skillSlot.Skill;
        }

        SkillSlot toSlot = to as SkillSlot;
        toSlot.Skill = skill;
        return true;
    }

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
