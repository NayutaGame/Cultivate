
using System;
using System.Collections.Generic;

public class PuzzleCell : Cell
{
    private Puzzle _puzzle;

    public string GetDescription() => _puzzle.Description;
    public string GetCondition() => _puzzle.Condition;
    public StageResult GetResult() => _puzzle?.GetResult();

    public PuzzleCell(Puzzle puzzle)
    {
        _accessors = new()
        {
            { "Guide",                    GetGuideDescriptor },
            { "Home",                     () => _puzzle.Home },
            { "Away",                     () => _puzzle.Away },
        };

        _puzzle = puzzle;

        foreach (SkillSlot slot in _puzzle.Home.TraversalCurrentSlots())
        {
            if (slot.Skill != null)
                slot.Skill.Borrowed = true;
        }
    }

    private void RemoveAllBorrowed()
    {
        RunEnvironment env = RunManager.Instance.Environment;

        RunEntity puzzleHome = _puzzle.Home;
        foreach (SkillSlot skillSlot in puzzleHome.TraversalCurrentSlots())
        {
            if (skillSlot.Skill is { Borrowed: true })
                skillSlot.Skill = null;
        }

        foreach (SkillSlot skillSlot in env.Home.TraversalCurrentSlots())
        {
            if (skillSlot.Skill is { Borrowed: true })
                skillSlot.Skill = null;
        }

        List<RunSkill> toRemove = new();
        foreach (RunSkill skill in env.Hand.Traversal())
        {
            if (skill is { Borrowed: true })
                toRemove.Add(skill);
        }

        foreach (RunSkill skill in toRemove)
        {
            env.Hand.Remove(skill);
        }
    }

    private void UnequipAll()
    {
        RunEnvironment env = RunManager.Instance.Environment;

        RunEntity puzzleHome = _puzzle.Home;
        foreach (SkillSlot skillSlot in puzzleHome.TraversalCurrentSlots())
        {
            if (skillSlot.Skill != null)
                env.LegacyUnequipProcedure(skillSlot, null);
        }
    }
    
    private Func<PuzzleResultSignal, Cell> _operation;
    public PuzzleCell SetOperation(Func<PuzzleResultSignal, Cell> operation)
    {
        _operation = operation;
        return this;
    }
    
    public override Cell DefaultReceiveSignal(Signal signal)
    {
        if (signal is PuzzleResultSignal puzzleResultSignal)
        {
            RemoveAllBorrowed();
            UnequipAll();
            // make sure puzzle is disposed
            // refresh hand and field
            return _operation(puzzleResultSignal);
        }
    
        return this;
    }

    public static PuzzleCell GetTemplate()
    {
        Puzzle puzzle = new(
            description: "尝试帮助少年击中目标",
            condition: "目标受到伤害",
            home: RunEntity.FromHardCoded(JingJie.LianQi, 14, 3),
            away: RunEntity.FromHardCoded(JingJie.LianQi, 1000000, 3, new[]
            {
                RunSkill.FromEntry("0609"),
                RunSkill.FromEntry("0609"),
                RunSkill.FromEntry("0609"),
            }),
            kernel: new StageKernel(async d =>
            {
                await d.Env.ClosureDict.SendEvent(StageClosureDict.WIL_COMMIT, d);

                if (d.Forced)
                {
                    d.Flag = d.Env.Entities[0].Hp > 0 ? 1 : 2;
                }
                else
                {
                    if (d.Cancel)
                        return 0;

                    if (d.Turn < 6)
                        return 0;

                    d.Flag = d.Env.Entities[0].Hp > 0 ? 1 : 2;
                }

                if (d.Flag == 0)
                    return d.Flag;

                await d.Env.ClosureDict.SendEvent(StageClosureDict.DID_COMMIT, d);

                d.Env.RecordResult(d.Flag);
                
                return d.Flag;
            })
        );
        
        PuzzleCell template = new(puzzle);
        DialogCell pass = new DialogCell("通过", "通过对话");
        DialogCell noPass = new DialogCell("未通过", "未通过对话");
        template.SetOperation(s =>
        {
            if (s.Flag == 1)
            {
                return pass;
            }

            return noPass;
        });
        
        return template;
    }
}
